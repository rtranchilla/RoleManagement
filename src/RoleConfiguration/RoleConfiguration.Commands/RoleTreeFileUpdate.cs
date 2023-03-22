using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RoleConfiguration.DataPersistence;
using RoleConfiguration.Repositories;
using RoleConfiguration.Yaml;
using RoleConfiguration.Yaml.Serialization;
using System.Data;
using System.Threading;

namespace RoleConfiguration.Commands;

public sealed record RoleTreeFileUpdate(string Source, string Path, string Content) : CommandRequest;

public sealed class RoleTreeFileUpdateHandler : IRequestHandler<RoleTreeFileUpdate>
{
    private readonly Deserializer deserializer;
    private readonly ConfigDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IRoleRepository roleRepository;
    private readonly ITreeRepository treeRepository;

    public RoleTreeFileUpdateHandler(Deserializer deserializer, ConfigDbContext dbContext, IMapper mapper, IRoleRepository roleRepository, ITreeRepository treeRepository)
    {
        this.deserializer = deserializer;
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.roleRepository = roleRepository;
        this.treeRepository = treeRepository;
    }

    public async Task<Unit> Handle(RoleTreeFileUpdate request, CancellationToken cancellationToken)
    {
        File file = GetFile(request);
        await dbContext.SaveChangesAsync(cancellationToken);
        var otherTreeIds = GetOtherFileTreeIds(file);
        var otherRoleIds = GetOtherFileRoleIds(file);
        using (IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken))
            try
            {
                RoleTreeFileContent fileContent = await GetFileContent(request, cancellationToken);

                var (treeAddList, treeUpdateList, treeRemoveList) = await GenerateTreeUpdates(file, fileContent, otherTreeIds, cancellationToken);

                await Update(treeUpdateList, treeAddList, treeRepository, cancellationToken);
                await Add(treeAddList, treeRepository, cancellationToken);
                await Remove(treeRemoveList, treeRepository, cancellationToken);

                var (roleAddList, roleUpdateList, roleRemoveList) = await GenerateRoleUpdates(file, fileContent, otherRoleIds, cancellationToken);

                await Update(roleUpdateList, roleAddList, roleRepository, cancellationToken);
                await Add(roleAddList, roleRepository, cancellationToken);
                await Remove(roleRemoveList, roleRepository, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            
        return Unit.Value;
    }

    private File GetFile(RoleTreeFileUpdate request)
    {
        var file = dbContext.Files!.Include(e => e.Trees)
                                   .Include(e => e.Roles)
                                   .FirstOrDefault(e => e.Path == request.Path && e.Source!.Name == request.Source);
        if (file == null)
        {
            var source = dbContext.Sources!.First(e => e.Name == request.Source);
            file = new File(request.Path, source.Id);
            dbContext.Files!.Add(file);
        }

        return file;
    }

    private Guid[] GetOtherFileTreeIds(File file)
    {
        var dataTable = new DataTable();
        using (var command = dbContext.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = $"Select [Id] from [dbo].[Trees] where Id not in (Select TreeId from [dbo].[FileTrees] where [FileId] <> '{file.Id}')";
            command.CommandType = CommandType.Text;

            dbContext.Database.OpenConnection();

            using var result = command.ExecuteReader();
            dataTable.Load(result);
        }
        var childFileTrees = new List<Guid>();
        foreach (DataRow row in dataTable.Rows)
            childFileTrees.Add((Guid)row[0]);
        return childFileTrees.ToArray();
    }

    private Guid[] GetOtherFileRoleIds(File file)
    {
        var dataTable = new DataTable();
        using (var command = dbContext.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = $"Select [Id] from [dbo].[Roles] where Id not in (Select RoleId from [dbo].[FileRoles] where [FileId] <> '{file.Id}')";
            command.CommandType = CommandType.Text;

            dbContext.Database.OpenConnection();

            using var result = command.ExecuteReader();
            dataTable.Load(result);
        }
        var childFileRoles = new List<Guid>();
        foreach (DataRow row in dataTable.Rows)
            childFileRoles.Add((Guid)row[0]);
        return childFileRoles.ToArray();
    }

    private async Task<RoleTreeFileContent> GetFileContent(RoleTreeFileUpdate request, CancellationToken cancellationToken)
    {
        var fileRecord = dbContext.Files!.FirstOrDefault(e => e.Path == request.Path && e.Source!.Name == request.Source);
        if (fileRecord == null)
        {
            var source = dbContext.Sources!.First(e => e.Name == request.Source);
            fileRecord = new File(request.Path, source.Id);
            await dbContext.Files!.AddAsync(fileRecord, cancellationToken);
        }

        var fileContent = deserializer.DeserializeRoleTree(request.Content, true);
        return fileContent;
    }

    private async Task<(List<(Tree Tree, TreeContent TreeContent)> AddList, List<(Tree Tree, TreeContent TreeContent)> UpdateList, List<Guid> RemoveList)> 
        GenerateTreeUpdates(File file, RoleTreeFileContent fileContent, Guid[] otherFileTreeIds, CancellationToken cancellationToken)
    {
        var currentTrees = dbContext.Trees!.Where(e => fileContent.Trees.Select(m => m.Name).Contains(e.Name)).ToDictionary(e => e.Name, StringComparer.InvariantCultureIgnoreCase);
        List<(Tree Tree, TreeContent TreeContent)> updateList = new();
        List<(Tree Tree, TreeContent TreeContent)> addList = new();
        List<Guid> currentIds = new();
        foreach (var treeContent in fileContent.Trees)
        {
            Tree? tree;
            if (currentTrees.TryGetValue(treeContent.Name ?? "", out tree!))
            {
                updateList.Add((tree, treeContent));
                if (!file.Trees.Any(e => e.TreeId == tree.Id))
                    file.Trees.Add(new FileTree(file.Id, tree.Id));
            }
            else
            {
                TreeContent? currentContent;
                try
                {
                    (tree, currentContent) = await treeRepository.Get(treeContent.Name!, cancellationToken);
                }
                catch (HttpRequestException ex)
                {
                    if (ex.StatusCode != System.Net.HttpStatusCode.NotFound)
                        throw;
                }

                if (tree == null)
                {
                    tree = mapper.Map<TreeContent, Tree>(treeContent);
                    addList.Add((tree, treeContent));
                }
                else
                    updateList.Add((tree, treeContent));

                await dbContext.Trees!.AddAsync(tree, cancellationToken);
                file.Trees.Add(new FileTree(file.Id, tree.Id));
                currentIds.Add(tree.Id);
            }
        }

        var removeList = otherFileTreeIds.Where(e => !currentIds.Contains(e)).ToList();
        return (addList, updateList, new List<Guid>());
    }

    private async Task<(List<(Role Role, RoleContent RoleContent)> AddList, List<(Role Role, RoleContent RoleContent)> UpdateList, List<Guid> RemoveList)> 
        GenerateRoleUpdates(File file, RoleTreeFileContent fileContent, Guid[] childFileRoleIds, CancellationToken cancellationToken)
    {
        var currentRoles = dbContext.Roles!.Where(e => fileContent.Roles.Select(m => m.Name).Contains(e.Name)).ToDictionary(e => e.Name, StringComparer.InvariantCultureIgnoreCase);
        List<(Role Role, RoleContent RoleContent)> updateList = new();
        List<(Role Role, RoleContent RoleContent)> addList = new();
        List<Guid> currentIds = new();
        foreach (var roleContent in fileContent.Roles)
        {
            Role? role;
            if (currentRoles.TryGetValue(roleContent.Name ?? "", out role!))
            {
                updateList.Add((role, roleContent));
                if (!file.Roles.Any(e => e.RoleId == role.Id))
                    file.Roles.Add(new FileRole(file.Id, role.Id));
            }
            else
            {
                RoleContent? currentContent;
                try
                {
                    (role, currentContent) = await roleRepository.Get(roleContent.Name!, roleContent.Tree!, cancellationToken);
                }
                catch (HttpRequestException ex)
                {
                    if (ex.StatusCode != System.Net.HttpStatusCode.NotFound)
                        throw;
                }

                if (role == null)
                {
                    Guid treeId = (await treeRepository.Get(roleContent.Tree!, cancellationToken)).Entity?.Id ?? throw new ArgumentNullException();
                    role = new Role(roleContent.Name!, treeId);
                    role = mapper.Map(roleContent, role);
                    addList.Add((role, roleContent));
                }
                else
                    updateList.Add((role, roleContent));

                await dbContext.Roles!.AddAsync(role, cancellationToken);
                file.Roles.Add(new FileRole(file.Id, role.Id));
                currentIds.Add(role.Id);
            }
        }

        var removeList = childFileRoleIds.Where(e => !currentIds.Contains(e)).ToList();
        return (addList, updateList, removeList);
    }

    private static async Task Add<TEntity, TContent>(List<(TEntity Entity, TContent Content)> addList, IRepository<TEntity, TContent> repository, CancellationToken cancellationToken)
    {
        foreach (var (entity, content) in addList)
            await repository.Add(entity, content, cancellationToken);
    }
    private async Task Update<TEntity, TContent>(List<(TEntity Entity, TContent Content)> addList, List<(TEntity Entity, TContent Content)> updateList, IRepository<TEntity, TContent> repository, CancellationToken cancellationToken)
    {
        foreach (var (entity, content) in updateList)
            try
            {
                await repository.Update(entity, content, cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    addList.Add((entity, content));
                else
                    throw;
            }

    }
    private static async Task Remove<TEntity, TContent>(List<Guid> ids, IRepository<TEntity, TContent> repository, CancellationToken cancellationToken)
    {
        foreach (var id in ids)
            await repository.Delete(id, cancellationToken);
    }
}
