namespace RoleManager.PowerShell.Requests;

public sealed record MemberCreate(Member Member) : RmCommandWithContent;

public sealed class MemberCreateHandler : RmCommandWithContentHandler<MemberCreate, Member, Dto.Member>
{
    public MemberCreateHandler(IHttpClientProvider httpClientFactory, IMapper mapper, IJsonSerializerSettingsProvider serializerSettingsProvider) : 
        base(httpClientFactory, mapper, serializerSettingsProvider) { }

    protected override Member GetEntity(MemberCreate request) => request.Member;
    protected override HttpMethod GetHttpMethod() => HttpMethod.Post;
    protected override Uri BuildRequestPath(Uri baseUri, MemberCreate request) => new(baseUri, "/Member");
}
