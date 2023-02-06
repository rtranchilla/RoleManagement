namespace RoleManager.PowerShell.Requests;

public sealed record MemberDelete(Guid Id) : RmCommand;

public sealed class MemberDeleteHandler : RmCommandHandler<MemberDelete>
{
    public MemberDeleteHandler(IHttpClientProvider httpClientFactory) : base(httpClientFactory) { }

    protected override HttpMethod GetHttpMethod() => HttpMethod.Delete;
    protected override Uri BuildRequestPath(Uri baseUri, MemberDelete request) => new(baseUri, $"/Member/{request.Id}");
}