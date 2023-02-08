namespace RoleManager.PowerShell.Requests;

public sealed record MemberRoleUpdate(Guid MemberId, Guid RoleId, Guid TreeId) : RmCommand;

public sealed class MemberRoleUpdateHandler : RmCommandHandler<MemberRoleUpdate>
{
    public MemberRoleUpdateHandler(IHttpClientProvider httpClientFactory) : base(httpClientFactory) { }

    protected override HttpMethod GetHttpMethod() => HttpMethod.Put;
    protected override Uri BuildRequestPath(Uri baseUri, MemberRoleUpdate request) => new(baseUri, $"/MemberRole/{request.MemberId}/{request.RoleId}/{request.TreeId}");
}