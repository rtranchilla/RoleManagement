namespace RoleManager.PowerShell.Requests;

public sealed record TreeDelete(Guid Id) : RmCommand;

public sealed class TreeDeleteHandler : RmCommandHandler<TreeDelete>
{
    public TreeDeleteHandler(IHttpClientProvider httpClientFactory) : base(httpClientFactory) { }

    protected override HttpMethod GetHttpMethod() => HttpMethod.Delete;
    protected override Uri BuildRequestPath(Uri baseUri, TreeDelete request) => new(baseUri, $"/Tree/{request.Id}");
}