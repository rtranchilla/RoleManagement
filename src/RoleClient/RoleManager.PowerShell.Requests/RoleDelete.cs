namespace RoleManager.PowerShell.Requests;

public sealed record RoleDelete(Guid Id) : RmCommand;

public sealed class RoleDeleteHandler : RmCommandHandler<RoleDelete>
{
    public RoleDeleteHandler(IHttpClientProvider httpClientFactory) : base(httpClientFactory) { }

    protected override HttpMethod GetHttpMethod() => HttpMethod.Delete;
    protected override Uri BuildRequestPath(Uri baseUri, RoleDelete request) => new(baseUri, $"/Role/{request.Id}");
}