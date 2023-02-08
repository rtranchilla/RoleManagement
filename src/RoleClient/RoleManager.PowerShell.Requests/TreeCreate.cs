namespace RoleManager.PowerShell.Requests;

public sealed record TreeCreate(Tree Tree) : RmCommandWithContent;

public sealed class TreeCreateHandler : RmCommandWithContentHandler<TreeCreate, Tree, Dto.Tree>
{
    public TreeCreateHandler(IHttpClientProvider httpClientFactory, IMapper mapper, IJsonSerializerSettingsProvider serializerSettingsProvider) : 
        base(httpClientFactory, mapper, serializerSettingsProvider) { }

    protected override Tree GetEntity(TreeCreate request) => request.Tree;
    protected override HttpMethod GetHttpMethod() => HttpMethod.Post;
    protected override Uri BuildRequestPath(Uri baseUri, TreeCreate request) => new(baseUri, "/Tree");
}
