namespace RoleManager.PowerShell.Requests;

public sealed record MemberQuery() : RmQuery<Member>
{
    public Guid? Id { get; set; }
    public Guid? RoleId { get; set; }
    public string? UniqueName { get; set; }
}

public sealed class MemberQueryHandler : RmQueryHandler<MemberQuery, Member, Dto.Member>
{
    public MemberQueryHandler(IHttpClientProvider httpClientProvider, IMapper mapper, IJsonSerializerSettingsProvider serializerSettingsProvider) : base(httpClientProvider, mapper, serializerSettingsProvider) { }

    protected override Uri BuildRequestPath(Uri baseUri, MemberQuery request)
    {
        if (request.Id != null)
            return new Uri(baseUri, $"/Member/{request.Id}");
        if (request.RoleId != null)
            return new Uri(baseUri, $"/Member/ByRole/{request.RoleId}");
        if (!string.IsNullOrEmpty(request.UniqueName))
            return new Uri(baseUri, $"/Member/ByUniqueName/{request.UniqueName}");

        return new Uri(baseUri, $"/Member");
    }
}