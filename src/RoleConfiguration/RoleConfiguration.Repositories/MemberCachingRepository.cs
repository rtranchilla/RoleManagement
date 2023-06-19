using AutoMapper;
using System.Text;

namespace RoleConfiguration.Repositories;

public sealed class MemberCachingRepository : Repository<Member, MemberContent, RoleManager.Dto.Member, MemberUpdate>, IMemberRepository
{
    private const string requestMemberRoleUri = "MemberRole";
    private const string requestUri = "Member";
    protected override string ControllerRequestUri => requestUri;

    public MemberCachingRepository(HttpClient roleManagerClient, JsonSerializerSettings serializerSettings, IMapper mapper, RoleCachingRepository roleRepository, TreeCachingRepository treeRepository) : base(roleManagerClient, serializerSettings, mapper)
    {
        this.roleManagerClient = roleManagerClient;
        this.roleRepository = roleRepository;
        this.treeRepository = treeRepository;
    }

    private readonly Dictionary<Guid, RoleManager.Dto.Member> byId = new();
    private readonly Dictionary<Guid, RoleManager.Dto.Member[]> byRoleId = new();
    private readonly Dictionary<string, RoleManager.Dto.Member> byName = new(StringComparer.InvariantCultureIgnoreCase);
    private readonly HttpClient roleManagerClient;
    private readonly RoleCachingRepository roleRepository;
    private readonly TreeCachingRepository treeRepository;

    public async Task<(Member? Entity, MemberContent? Content)> Get(string uniqueName, CancellationToken cancellationToken = default)
    {
        if (!byName.TryGetValue(uniqueName, out RoleManager.Dto.Member? dto))
        {
            dto = await GetByUri($"{requestUri}/ByUniqueName/{uniqueName}", cancellationToken);
            if (dto != null)
            {
                byId.TryAdd(dto.Id, dto);
                byName.TryAdd(uniqueName, dto);
            }
        }

        return await MapToResult(dto, cancellationToken);
    }

    public override async Task<(Member? Entity, MemberContent? Content)> Get(Guid id, CancellationToken cancellationToken = default)
    {
        if (!byId.TryGetValue(id, out RoleManager.Dto.Member? dto))
        {
            dto = await GetByUri($"{requestUri}/{id}", cancellationToken);
            if (dto != null)
            {
                byId.TryAdd(id, dto);
                byName.TryAdd(dto.UniqueName, dto);
            }
        }

        return await MapToResult(dto, cancellationToken);
    }

    public async Task<IEnumerable<(Member? Entity, MemberContent? Content)>> GetByRole(Guid id, CancellationToken cancellationToken = default)
    {
        if (!byRoleId.TryGetValue(id, out RoleManager.Dto.Member[]? dtos))
        {
            dtos = await GetCollectionByUri($"{requestUri}/ByRole/{id}", cancellationToken) ?? Array.Empty<RoleManager.Dto.Member>();
            if (dtos != null)
            {
                byRoleId.TryAdd(id, dtos);
                foreach (var dto in dtos)
                {
                    byId.TryAdd(dto.Id, dto);
                    byName.TryAdd(dto.UniqueName, dto);
                }
            }
        }

        var result = new List<(Member? Entity, MemberContent? Content)>();
        foreach (RoleManager.Dto.Member dto in dtos!)
            result.Add(await MapToResult(dto, cancellationToken));

        return result;
    }

    private async Task<(Member? Entity, MemberContent? Content)> MapToResult(RoleManager.Dto.Member? dto, CancellationToken cancellationToken)
    {
        var (entity, content) = MapFromDto(dto);

        if (content != null)
            foreach (var (_, roleContent) in await roleRepository.GetByMember(dto!.Id, cancellationToken))
                if (roleContent != null)
                    content.Roles.Add(roleContent);

        return (entity, content);
    }

    public override async Task Add(Member entity, MemberContent content, CancellationToken cancellationToken = default)
    {
        await base.Add(entity, content, cancellationToken);
        await UpdateRoles(entity, content, cancellationToken);
    }

    public override async Task Update(Member entity, MemberContent content, CancellationToken cancellationToken = default)
    {
        await base.Update(entity, content, cancellationToken);
        await UpdateRoles(entity, content, cancellationToken);
    }

    private async Task UpdateRoles(Member entity, MemberContent content, CancellationToken cancellationToken)
    {
        HashSet<(string Name, string Tree)> currentRoles = (await roleRepository.GetByMember(entity!.Id, cancellationToken)).Where(e => e.Entity != null).Select(e => (e.Entity!.Name.ToUpperInvariant(), e.Content!.Tree!.ToUpperInvariant())).ToHashSet();
        HashSet<(string Name, string Tree)> updateRoles = content.Roles.Where(e => e.Name != null).Select(e => (e.Name!.ToUpperInvariant(), e.Tree!.ToUpperInvariant())).ToHashSet();

        foreach (var role in content.Roles)
            if (role?.Name != null && !currentRoles.Contains((role.Name, role.Tree!)))
            {
                var (roleEntity, roleContent) = await roleRepository.Get(role.Name, role.Tree!, cancellationToken);
                if (roleEntity != null)
                {
                    HttpResponseMessage response = await roleManagerClient.PutAsync($"{requestMemberRoleUri}/{entity.Id}/{roleEntity.Id}/{roleEntity.TreeId}", null, cancellationToken);
                    response.EnsureSuccessStatusCode();
                }
            }

        foreach (var role in currentRoles)
            if (!updateRoles.Contains(role))
            {
                var (treeEntity, _) = await treeRepository.Get(role.Tree, cancellationToken);
                if (treeEntity != null)
                {
                    HttpResponseMessage response = await roleManagerClient.DeleteAsync($"{requestMemberRoleUri}/{entity.Id}/{treeEntity.Id}", cancellationToken);
                    response.EnsureSuccessStatusCode();
                }
            }

        roleRepository.ClearMemberCache(entity.Id);
    }
}