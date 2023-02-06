namespace RoleManager.PowerShell.Requests;

public abstract record RmRequest<TResponse> : IRequest<TResponse>
{
    public Uri? ConnectionUri { get; set; }
}

public abstract class RmRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : RmRequest<TResponse>
{
    private readonly IHttpClientProvider httpClientProvider;

    public RmRequestHandler(IHttpClientProvider httpClientProvider) => this.httpClientProvider = httpClientProvider;

    protected virtual Uri BuildRequestPath(Uri baseUri, TRequest request) => baseUri;
    protected Uri GetUri(TRequest request) => BuildRequestPath(request.ConnectionUri ?? throw new ArgumentNullException(nameof(request.ConnectionUri)), request);
    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default) => 
        Handle(request, httpClientProvider.Get(), cancellationToken);

    protected abstract Task<TResponse> Handle(TRequest request, HttpClient httpClient, CancellationToken cancellationToken);
}