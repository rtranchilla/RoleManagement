using AutoMapper;
using System.Diagnostics;

namespace RoleConfiguration.Repositories;

public abstract class Repository<TEntity, TContent, TDto, TUpdateDto> : IRepository<TEntity, TContent>
    where TEntity : class
    where TContent : class
    where TDto : class, RoleManager.Dto.IEntity
    where TUpdateDto : class, RoleManager.Dto.IEntity
{
    private readonly System.Text.Encoding encoding = System.Text.Encoding.UTF8;
    private const string mediaType = "application/json";
    protected abstract string ControllerRequestUri { get; }

    public Repository(HttpClient roleManagerClient, JsonSerializerSettings serializerSettings, IMapper mapper)
    {
        this.roleManagerClient = roleManagerClient;
        this.serializerSettings = serializerSettings;
        this.mapper = mapper;
    }

    private readonly HttpClient roleManagerClient;
    private readonly JsonSerializerSettings serializerSettings;
    private readonly IMapper mapper;

    public abstract Task<(TEntity? Entity, TContent? Content)> Get(Guid id, CancellationToken cancellationToken = default);
    protected async Task<TDto?> GetByUri(string completeRequestUri, CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await roleManagerClient.GetAsync(completeRequestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<TDto[]>(responseBody, serializerSettings)!.FirstOrDefault();
    }
    protected async Task<TDto[]?> GetCollectionByUri(string completeRequestUri, CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await roleManagerClient.GetAsync(completeRequestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<TDto[]>(responseBody, serializerSettings);
    }

    protected (TEntity? Entity, TContent? Content) MapFromDto(TDto? dto)
    {
        if (dto == null)
            return (null, null);

        var entity = mapper.Map<TDto, TEntity>(dto);
        var content = mapper.Map<TDto, TContent>(dto);
        return (entity, content);
    }

    public async Task Add(TEntity entity, TContent content, CancellationToken cancellationToken = default)
    {
        TDto dto = mapper.Map<TEntity, TDto>(entity);
        dto = mapper.Map(content, dto);

        HttpResponseMessage response = await roleManagerClient.PostAsync(ControllerRequestUri, new StringContent(JsonConvert.SerializeObject(dto, serializerSettings), encoding, mediaType), cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task Update(TEntity entity, TContent content, CancellationToken cancellationToken = default)
    {
        TUpdateDto dto = mapper.Map<TEntity, TUpdateDto>(entity);
        dto = mapper.Map(content, dto);

        HttpResponseMessage response = await roleManagerClient.PutAsync(ControllerRequestUri, new StringContent(JsonConvert.SerializeObject(dto, serializerSettings), encoding, mediaType), cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await roleManagerClient.DeleteAsync($"{ControllerRequestUri}/{id}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
