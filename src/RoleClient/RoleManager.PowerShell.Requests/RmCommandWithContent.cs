using RoleManager.Dto;
using System.Net.Http.Headers;

namespace RoleManager.PowerShell.Requests;

public abstract record RmCommandWithContent : RmCommand;

public abstract class RmCommandWithContentHandler<TRequest, TEntity, TDto> : RmCommandHandler<TRequest>
    where TRequest : RmCommandWithContent
    where TEntity : class
    where TDto : class, IEntity
{
    protected RmCommandWithContentHandler(IHttpClientProvider httpClientFactory, IMapper mapper, IJsonSerializerSettingsProvider serializerSettingsProvider) : base(httpClientFactory)
    {
        this.mapper = mapper;
        this.serializerSettings = serializerSettingsProvider.Get();
    }

    private readonly IMapper mapper;
    private readonly JsonSerializerSettings serializerSettings;

    protected abstract TEntity GetEntity(TRequest request);

    protected override Task BuildMessage(TRequest request, HttpRequestMessage httpRequestMessage) =>
        Task.Run(() =>
        {
            var dto = mapper.Map<TEntity, TDto>(GetEntity(request));
            var contentBody = JsonConvert.SerializeObject(dto, serializerSettings);
            httpRequestMessage.Content = new StringContent(contentBody, System.Text.Encoding.Default, "application/json");
        });
}