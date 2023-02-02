using AutoMapper;

namespace RoleManager.PowerShell.Requests;

public sealed record MemberQuery() : Query<Member>
{
    public Guid? Id { get; set; }
    public Guid? RoleId { get; set; }
    public string? UniqueName { get; set; }
}

public sealed class MemberQueryHandler : QueryHandler<MemberQuery, Member, Dto.Member>
{
    public MemberQueryHandler(HttpClient httpClient, IMapper mapper) : base(httpClient, mapper) { }

    protected override string GetPath() => "http://localhost:60301/Member"; 
    protected override string GetQueryString(MemberQuery request)
    {
        if (request.Id != null)
            return $"/ById/{request.Id}";
        if (request.RoleId != null)
            return $"/ByRole/{request.RoleId}";
        if (!string.IsNullOrEmpty(request.UniqueName))
            return $"/ByUniqueName/{request.UniqueName}";

        return "";
    }
}