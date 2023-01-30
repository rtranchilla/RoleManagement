using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RoleManager.DataPersistence;
using RoleManager.RoleHierarchy;
using System.Data;
using System.Linq;

namespace RoleManager.Queries;

public sealed record RoleAvailableQuery(Guid MemberId) : AggregateRootQuery<Dto.Role>;

public sealed class RoleAvailableQueryHandler : AggregateRootQueryHandler<RoleAvailableQuery, Role, Dto.Role>
{
    public RoleAvailableQueryHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

    protected override IEnumerable<Role> QueryEntities(RoleAvailableQuery request, RoleDbContext dbContext)
    {
        var member = dbContext.Members!.IncludeSubordinate(true).FirstOrDefault(e => e.Id == request.MemberId);
        if (member == null)
            throw new ArgumentException("Member specified was not found,", nameof(request.MemberId));

        return ForestMap.Build(dbContext.Roles!.IncludeSubordinate(true)).GetAvailableRoles(member);
    }
}