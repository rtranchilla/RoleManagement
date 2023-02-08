namespace RoleManager.PowerShell.Requests;

public sealed record RoleUpdate(Role Role) : RmCommandWithContent;

public sealed class RoleUpdateHandler : RmCommandWithContentHandler<RoleUpdate, Role, Dto.RoleUpdate>
{
    public RoleUpdateHandler(IHttpClientProvider httpClientFactory, IMapper mapper, IJsonSerializerSettingsProvider serializerSettingsProvider) : 
        base(httpClientFactory, mapper, serializerSettingsProvider) { }

    protected override Role GetEntity(RoleUpdate request) => request.Role;
    protected override HttpMethod GetHttpMethod() => HttpMethod.Put;
    protected override Uri BuildRequestPath(Uri baseUri, RoleUpdate request) => new(baseUri, "/Role");
}
