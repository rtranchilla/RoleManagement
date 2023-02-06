namespace RoleManager.PowerShell.Requests;

public sealed record TreeQuery : RmQuery<Tree>
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
}

public sealed class TreeQueryHandler : RmQueryHandler<TreeQuery, Tree, Dto.Tree>
{
    public TreeQueryHandler(IHttpClientProvider httpClientProvider, IMapper mapper, IJsonSerializerSettingsProvider serializerSettingsProvider) : base(httpClientProvider, mapper, serializerSettingsProvider) { }

    protected override Uri BuildRequestPath(Uri baseUri, TreeQuery request)
    {
        if (request.Id != null)
            return new Uri(baseUri, $"/Member/{request.Id}");
        if (!string.IsNullOrEmpty(request.Name))
            return new Uri(baseUri, $"/Member/ByName/{request.Name}");

        return new Uri(baseUri, $"/Member");
    }
}