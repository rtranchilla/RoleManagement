using AutoMapper;
using Newtonsoft.Json;
using RoleManager.Dto;
using System.Net;

namespace RoleManager.PowerShell.Requests;

public abstract record Query<TResult> : IRequest<TResult[]>;

public abstract class QueryHandler<TRequest, TPs, TDto> : IRequestHandler<TRequest, TPs[]>
    where TRequest : Query<TPs>
    where TPs : class
    where TDto : class, IEntity
{
    public QueryHandler(HttpClient httpClient, IMapper mapper)
    {
        this.httpClient = httpClient;
        this.mapper = mapper;
    }

    protected abstract string GetPath();
    protected abstract string GetQueryString(TRequest request);

    public async Task<TPs[]> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var uriAddress = new Uri($"{address}{GetPath()}{GetQueryString(request)}");
        using HttpResponseMessage response = await httpClient.GetAsync(uriAddress, cancellationToken);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
            throw new ArgumentException("No item(s) not found.");

        if ((int)response.StatusCode == 500)
            throw new Exception("Internal server error.");

        if (response.StatusCode != HttpStatusCode.OK)
            throw new Exception("Unknown error.");

        if (!string.IsNullOrEmpty(responseBody))
        {
            var deseralizedResponseBody = JsonConvert.DeserializeObject<TDto[]>(responseBody, serializerSettings);
            if (deseralizedResponseBody != null)
                return mapper.Map<TDto[], TPs[]>(deseralizedResponseBody);
        }

        return Array.Empty<TPs>();
    }

    //private ICredentials credentials;
    private string address;
    private readonly JsonSerializerSettings serializerSettings;
    private readonly HttpClient httpClient;
    public IMapper mapper;

    public void SetAddress(string address)
    {
        if (!address.EndsWith("/"))
            address += "/";
        address += GetPath();
        this.address = address;
    }

    //public void SetCredential(ICredentials credentials) => this.credentials = credentials;
}