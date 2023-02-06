namespace RoleManager.PowerShell.Requests;

public sealed record NodeQuery : RmQuery<Node>
{
    public Guid? Id { get; set; }
    public Guid? RoleId { get; set; }
    public string? Name { get; set; }
}

public sealed class NodeQueryHandler : RmQueryHandler<NodeQuery, Node, Dto.Node>
{
    public NodeQueryHandler(IHttpClientProvider httpClientProvider, IMapper mapper, IJsonSerializerSettingsProvider serializerSettingsProvider) : base(httpClientProvider, mapper, serializerSettingsProvider) { }

    protected override Uri BuildRequestPath(Uri baseUri, NodeQuery request)
    {
        if (request.Id != null)
            return new Uri(baseUri, $"/Node/{request.Id}");
        if (request.RoleId != null)
            return new Uri(baseUri, $"/Node/ByRole/{request.RoleId}");
        if (!string.IsNullOrEmpty(request.Name))
            return new Uri(baseUri, $"/Node/ByName/{request.Name}");

        return new Uri(baseUri, $"/Node");
    }
}