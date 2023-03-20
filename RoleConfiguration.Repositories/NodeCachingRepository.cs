namespace RoleConfiguration.Repositories;

public sealed class NodeCachingRepository
{
    public NodeCachingRepository(HttpClient roleManagerClient, JsonSerializerSettings serializerSettings)
    {
        this.roleManagerClient = roleManagerClient;
        this.serializerSettings = serializerSettings;
    }

    private readonly HttpClient roleManagerClient;
    private readonly JsonSerializerSettings serializerSettings;
    readonly Dictionary<Guid, Node> byId = new();
    readonly Dictionary<string, Node> byName = new(StringComparer.InvariantCultureIgnoreCase);

    internal async Task<Node?> Get(string name, Guid treeId, CancellationToken cancellationToken)
    {
        if (!byName.TryGetValue(name, out Node? dto))
        {
            using HttpResponseMessage response = await roleManagerClient.GetAsync($"Node/ByName/{name}/{treeId}", cancellationToken);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            dto = JsonConvert.DeserializeObject<Node[]>(responseBody, serializerSettings)!.FirstOrDefault();

            if (dto != null)
            {
                byId.TryAdd(dto.Id, dto);
                byName.TryAdd(name, dto);
            }
        }

        return dto;
    }

    internal async Task<Node?> Get(Guid id, CancellationToken cancellationToken)
    {
        if (!byId.TryGetValue(id, out Node? dto))
        {
            using HttpResponseMessage response = await roleManagerClient.GetAsync($"Node/{id}", cancellationToken);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            dto = JsonConvert.DeserializeObject<Node[]>(responseBody, serializerSettings)!.FirstOrDefault();

            if (dto != null)
            {
                byId.TryAdd(id, dto);
                byName.TryAdd(dto.Name, dto);
            }
        }

        return dto;
    }
}
