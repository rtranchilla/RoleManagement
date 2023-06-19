using AutoMapper;

namespace RoleConfiguration.Repositories;

public sealed class TreeCachingRepository : Repository<Tree, TreeContent, RoleManager.Dto.Tree, TreeUpdate>, ITreeRepository
{
    private const string requestUri = "Tree";
    private readonly IMapper mapper;
    private readonly NodeCachingRepository nodeRepository;

    protected override string ControllerRequestUri => requestUri;

    public TreeCachingRepository(HttpClient roleManagerClient, JsonSerializerSettings serializerSettings, IMapper mapper, NodeCachingRepository nodeRepository) : base(roleManagerClient, serializerSettings, mapper)
    {
        this.mapper = mapper;
        this.nodeRepository = nodeRepository;
    }

    readonly Dictionary<Guid, RoleManager.Dto.Tree> byId = new();
    readonly Dictionary<string, RoleManager.Dto.Tree> byName = new(StringComparer.InvariantCultureIgnoreCase);

    public async Task<(Tree? Entity, TreeContent? Content)> Get(string name, CancellationToken cancellationToken = default)
    {
        if (!byName.TryGetValue(name, out RoleManager.Dto.Tree? dto))
        {
            dto = await GetByUri($"{requestUri}/ByName/{name}", cancellationToken);
            if (dto != null)
            {
                byId.TryAdd(dto.Id, dto);
                byName.TryAdd(name, dto);
            }
        }

        return await MapToResult(dto, cancellationToken);
    }

    public async override Task<(Tree? Entity, TreeContent? Content)> Get(Guid id, CancellationToken cancellationToken = default)
    {
        RoleManager.Dto.Tree? dto = await GetById(id, cancellationToken);
        return await MapToResult(dto, cancellationToken);
    }

    private async Task<RoleManager.Dto.Tree?> GetById(Guid id, CancellationToken cancellationToken)
    {
        if (!byId.TryGetValue(id, out RoleManager.Dto.Tree? dto))
        {
            dto = await GetByUri($"{requestUri}/{id}", cancellationToken);
            if (dto != null)
            {
                byId.TryAdd(id, dto);
                byName.TryAdd(dto.Name, dto);
            }
        }

        return dto;
    }

    private async Task<(Tree? Entity, TreeContent? Content)> MapToResult(RoleManager.Dto.Tree? dto, CancellationToken cancellationToken)
    {
        var (entity, content) = MapFromDto(dto);
        if (content != null)
            foreach (var id in dto!.RequiredNodes)
            {
                var node = await nodeRepository.Get(id, cancellationToken);
                if (node != null)
                {
                    var requiredNode = mapper.Map<Node, RequiredNodeContent>(node);
                    requiredNode.Tree = (await GetById(id, cancellationToken))?.Name;
                    content.RequiredNodes.Add(requiredNode);
                }
            }
        
        return (entity, content);
    }
}
