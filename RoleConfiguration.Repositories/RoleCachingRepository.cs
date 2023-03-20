using AutoMapper;

namespace RoleConfiguration.Repositories;

public sealed class RoleCachingRepository : Repository<Role, RoleContent, RoleManager.Dto.Role, RoleUpdate>, IRoleRepository
{
    private const string requestUri = "Role";
    protected override string ControllerRequestUri => requestUri;

    public RoleCachingRepository(HttpClient roleManagerClient, JsonSerializerSettings serializerSettings, IMapper mapper, NodeCachingRepository nodeRepository, TreeCachingRepository treeRepository) : base(roleManagerClient, serializerSettings, mapper)
    {
        this.mapper = mapper;
        this.nodeRepository = nodeRepository;
        this.treeRepository = treeRepository;
    }

    private readonly Dictionary<Guid, RoleManager.Dto.Role> byId = new();
    private readonly Dictionary<Guid, RoleManager.Dto.Role[]> byMemberId = new();
    private readonly Dictionary<string, RoleManager.Dto.Role> byName = new(StringComparer.InvariantCultureIgnoreCase);
    private readonly IMapper mapper;
    private readonly NodeCachingRepository nodeRepository;
    private readonly TreeCachingRepository treeRepository;

    public async Task<(Role? Entity, RoleContent? Content)> Get(string name, string tree, CancellationToken cancellationToken = default)
    {
        if (!byName.TryGetValue(name, out RoleManager.Dto.Role? dto))
        {
            dto = await GetByUri($"{requestUri}/ByName/{name}/{tree}", cancellationToken);
            if (dto != null)
            {
                byId.TryAdd(dto.Id, dto);
                byName.TryAdd(name, dto);
            }
        }

        return await MapToResult(dto, cancellationToken);
    }

    public override async Task<(Role? Entity, RoleContent? Content)> Get(Guid id, CancellationToken cancellationToken = default)
    {
        if (!byId.TryGetValue(id, out RoleManager.Dto.Role? dto))
        {
            dto = await GetByUri($"{requestUri}/{id}", cancellationToken);
            if (dto != null)
            {
                byId.TryAdd(id, dto);
                byName.TryAdd(dto.Name, dto);
            }
        }

        return await MapToResult(dto, cancellationToken);
    }

    public async Task<IEnumerable<(Role? Entity, MemberRoleContent? Content)>> GetByMember(Guid id, CancellationToken cancellationToken = default)
    {
        if (!byMemberId.TryGetValue(id, out RoleManager.Dto.Role[]? dtos))
        {
            dtos = await GetCollectionByUri($"{requestUri}/ByMember/{id}", cancellationToken) ?? Array.Empty<RoleManager.Dto.Role>();
            if (dtos != null)
            {
                byMemberId.TryAdd(id, dtos);
                foreach (var dto in dtos)
                {
                    byId.TryAdd(dto.Id, dto);
                    byName.TryAdd(dto.Name, dto);
                }
            }
        }

        var result = new List<(Role? Entity, MemberRoleContent? Content)>();
        foreach (RoleManager.Dto.Role dto in dtos!)
            result.Add(await MapMemberContent(dto));

        return result;

        async Task<(Role? Entity, MemberRoleContent? Content)> MapMemberContent(RoleManager.Dto.Role dto)
        {
            if (dto == null)
                return (null, null);

            var entity = mapper.Map<RoleManager.Dto.Role, Role>(dto);
            var content = mapper.Map<RoleManager.Dto.Role, MemberRoleContent>(dto);
            content.Tree = (await treeRepository.Get(dto.TreeId, cancellationToken)).Entity?.Name;
            return (entity, content);
        }
    }

    private async Task<(Role? Entity, RoleContent? Content)> MapToResult(RoleManager.Dto.Role? dto, CancellationToken cancellationToken)
    {
        var (entity, content) = MapFromDto(dto);
        if (content != null)
            foreach (var id in dto!.RequiredNodes)
            {
                var node = await nodeRepository.Get(id, cancellationToken);
                if (node != null)
                {
                    var requiredNode = mapper.Map<Node, RequiredNodeContent>(node);
                    requiredNode.Tree = (await treeRepository.Get(node.TreeId, cancellationToken)).Entity?.Name;
                    content.RequiredNodes.Add(requiredNode);
                }
            }

        return (entity, content);
    }
}
