namespace RoleManager.PowerShell.Requests;

public sealed record TreeUpdate(Tree Tree) : RmCommandWithContent;

public sealed class TreeUpdateHandler : RmCommandWithContentHandler<TreeUpdate, Tree, Dto.TreeUpdate>
{
    public TreeUpdateHandler(IHttpClientProvider httpClientFactory, IMapper mapper, IJsonSerializerSettingsProvider serializerSettingsProvider) : 
        base(httpClientFactory, mapper, serializerSettingsProvider) { }

    protected override Tree GetEntity(TreeUpdate request) => request.Tree;
    protected override HttpMethod GetHttpMethod() => HttpMethod.Put;
    protected override Uri BuildRequestPath(Uri baseUri, TreeUpdate request) => new(baseUri, "/Tree");
}
