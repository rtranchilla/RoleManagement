using MediatR;
using RoleConfiguration.Repositories;
using RoleConfiguration.Yaml;
using RoleConfiguration.Yaml.Serialization;
using RoleManager.Queries;

namespace RoleConfiguration.Queries;

public sealed record RoleTreeFileQuery : QueryRequest<string>
{
    public RoleTreeFileQuery(params (string Name, string Tree)[] roles) => Roles = roles;
    public RoleTreeFileQuery(string tree) => Tree = tree;

    public (string Name, string Tree)[]? Roles { get; }
    public string? Tree { get; }
}

public sealed class RoleTreeFileQueryHandler : IRequestHandler<RoleTreeFileQuery, string>
{
    private readonly Serializer serializer;
    private readonly IRoleRepository roleRepository;
    private readonly ITreeRepository treeRepository;

    public RoleTreeFileQueryHandler(Serializer serializer, IRoleRepository roleRepository, ITreeRepository treeRepository)
    {
        this.serializer = serializer;
        this.roleRepository = roleRepository;
        this.treeRepository = treeRepository;
    }

    public async Task<string> Handle(RoleTreeFileQuery request, CancellationToken cancellationToken)
    {
        var content = new RoleTreeFileContent();
        if (!string.IsNullOrEmpty(request.Tree))
            await AddSpecifiedTree(request, content, cancellationToken);
        else if (request.Roles?.Any() ?? false)
            await AddSpecifiedRoles(request, content, cancellationToken);

        return serializer.Serialize(content, true);
    }

    private async Task AddSpecifiedTree(RoleTreeFileQuery request, RoleTreeFileContent content, CancellationToken cancellationToken)
    {
        var treeContent = (await treeRepository.Get(request.Tree!, cancellationToken)).Content;
        if (treeContent != null)
        {
            content.Trees.Add(treeContent);
            content.DefaultRoleTree = treeContent.Name;

            foreach (var (_, roleContent) in await roleRepository.GetByTree(request.Tree!, cancellationToken))
                if (roleContent != null)
                    content.Roles.Add(roleContent);
        }
    }

    // ToDo: Fill Tree Info
    private async Task AddSpecifiedRoles(RoleTreeFileQuery request, RoleTreeFileContent content, CancellationToken cancellationToken)
    {
        foreach (var (roleName, roleTree) in request.Roles!)
        {
            var (_, roleContent) = await roleRepository.Get(roleName, roleTree, cancellationToken);
            if (roleContent != null)
            {
                content.Roles.Add(roleContent);
            }
        }
    }
}

