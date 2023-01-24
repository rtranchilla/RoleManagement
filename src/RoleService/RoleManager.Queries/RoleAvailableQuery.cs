using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoleManager.DataPersistence;

namespace RoleManager.Queries;

public sealed record RoleAvailableQuery(Guid MemberId) : AggregateRootQuery<Dto.Role>;

public sealed class RoleAvailableQueryHandler : AggregateRootQueryHandler<RoleAvailableQuery, Role, Dto.Role>
{
    public RoleAvailableQueryHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

    protected override IQueryable<Role> QueryEntities(RoleAvailableQuery request, RoleDbContext dbContext)
    {
        IQueryable<Role> query = dbContext.Roles!.Include(e => e.Nodes.OrderBy(rn => rn.Order))
                                                 .ThenInclude(e => e.Node);

        //if (request.Id != null)
        //    return query.Where(e => e.Id == request.Id);

        return query;
    }
}