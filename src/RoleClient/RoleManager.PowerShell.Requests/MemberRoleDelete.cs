namespace RoleManager.PowerShell.Requests;

public sealed record MemberRoleDelete(Guid MemberId, Guid TreeId) : RmCommand;

public sealed class MemberRoleDeleteHandler : RmCommandHandler<MemberRoleDelete>
{
    public MemberRoleDeleteHandler(IHttpClientProvider httpClientFactory) : base(httpClientFactory) { }

    protected override HttpMethod GetHttpMethod() => HttpMethod.Delete;
    protected override Uri BuildRequestPath(Uri baseUri, MemberRoleDelete request) => new(baseUri, $"/MemberRole/{request.MemberId}/{request.TreeId}");
}