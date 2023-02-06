namespace RoleManager.PowerShell.Requests;

public sealed record RoleQuery : RmQuery<Role>
{
    public Guid? Id { get; set; }
    public Guid? MemberId { get; set; }
    public Guid? AvailableForMemberId { get; set; }
}

public sealed class RoleQueryHandler : RmQueryHandler<RoleQuery, Role, Dto.Role>
{
    public RoleQueryHandler(IHttpClientProvider httpClientProvider, IMapper mapper, IJsonSerializerSettingsProvider serializerSettingsProvider) : base(httpClientProvider, mapper, serializerSettingsProvider) { }

    protected override Uri BuildRequestPath(Uri baseUri, RoleQuery request)
    {
        if (request.Id != null)
            return new Uri(baseUri, $"/Role/{request.Id}");
        if (request.MemberId != null)
            return new Uri(baseUri, $"/Role/ByMember/{request.MemberId}");
        if (request.MemberId != null)
            return new Uri(baseUri, $"/Role/AvailableForMember/{request.AvailableForMemberId}");

        return new Uri(baseUri, $"/Role");
    }
}