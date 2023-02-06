using RoleManager.Dto;

namespace RoleManager.PowerShell.Requests;

public abstract record RmQuery<TResult> : RmRequest<TResult[]>;

public abstract class RmQueryHandler<TRequest, TEntity, TDto> : RmRequestHandler<TRequest, TEntity[]>
    where TRequest : RmQuery<TEntity>
    where TEntity : class
    where TDto : class, IEntity
{
    protected RmQueryHandler(IHttpClientProvider httpClientFactory, IMapper mapper, IJsonSerializerSettingsProvider serializerSettingsProvider) : base(httpClientFactory)
    {
        this.mapper = mapper;
        serializerSettings = serializerSettingsProvider.Get();
    }

    private readonly JsonSerializerSettings serializerSettings;
    private readonly IMapper mapper;

    protected override async Task<TEntity[]> Handle(TRequest request, HttpClient httpClient, CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await httpClient.GetAsync(GetUri(request), cancellationToken);

        if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
            throw new ArgumentException("No item(s) not found.");

        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!string.IsNullOrEmpty(responseBody))
            try
            {
                var deseralizedResponseBody = JsonConvert.DeserializeObject<TDto[]>(responseBody, serializerSettings);
                if (deseralizedResponseBody != null)
                    return mapper.Map<TDto[], TEntity[]>(deseralizedResponseBody);
            }
            catch (Exception ex)
            {
                throw new Exception("Falied to deseralize or map result of query. This may be caused by an invalid connection address or a version mismatch.", ex);
            }

        return Array.Empty<TEntity>();
    }
}