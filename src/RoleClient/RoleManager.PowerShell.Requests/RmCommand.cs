using System.ComponentModel.Design;

namespace RoleManager.PowerShell.Requests;

public abstract record RmCommand : RmRequest<Unit>;

public abstract class RmCommandHandler<TRequest> : RmRequestHandler<TRequest, Unit>
    where TRequest : RmCommand
{
    protected RmCommandHandler(IHttpClientProvider httpClientFactory) : base(httpClientFactory) { }

    protected abstract HttpMethod GetHttpMethod();
    protected virtual Task BuildMessage(TRequest request, HttpRequestMessage httpRequestMessage) => Task.CompletedTask; 

    protected override async Task<Unit> Handle(TRequest request, HttpClient httpClient, CancellationToken cancellationToken)
    {
        using var httpRequest = new HttpRequestMessage(GetHttpMethod(), GetUri(request));
        await BuildMessage(request, httpRequest);
        using HttpResponseMessage response = await httpClient.SendAsync(httpRequest, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
            throw new ArgumentException("No item(s) not found.");

        response.EnsureSuccessStatusCode();

        return Unit.Value;
    }
}