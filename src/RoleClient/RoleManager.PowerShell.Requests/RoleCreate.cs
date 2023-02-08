namespace RoleManager.PowerShell.Requests;

public sealed record RoleCreate(Role Role) : RmCommandWithContent;

public sealed class RoleCreateHandler : RmCommandWithContentHandler<RoleCreate, Role, Dto.Role>
{
    public RoleCreateHandler(IHttpClientProvider httpClientFactory, IMapper mapper, IJsonSerializerSettingsProvider serializerSettingsProvider) : 
        base(httpClientFactory, mapper, serializerSettingsProvider) { }

    protected override Role GetEntity(RoleCreate request) => request.Role;
    protected override HttpMethod GetHttpMethod() => HttpMethod.Post;
    protected override Uri BuildRequestPath(Uri baseUri, RoleCreate request) => new(baseUri, "/Role");
}
