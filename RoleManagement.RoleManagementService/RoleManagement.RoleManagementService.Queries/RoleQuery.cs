using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoleManagement.RoleManagementService.DataPersistence;

namespace RoleManagement.RoleManagementService.Queries;

public sealed record RoleQuery : AggregateRootQuery<Dto.Role>
{
    public RoleQuery() { }
    public RoleQuery(Guid id) => Id = id;

    public Guid? Id { get; }
    public Guid? MemberId { get; set; }
}

public sealed class RoleQueryHandler : AggregateRootQueryHandler<RoleQuery, Role, Dto.Role>
{
    public RoleQueryHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

    protected override IQueryable<Role> QueryEntities(RoleQuery request, RoleDbContext dbContext)
    {
        IQueryable<Role> query = dbContext.Roles!.Include(e => e.Nodes.OrderBy(rn => rn.Order))
                                                 .ThenInclude(e => e.Node);

        if (request.Id != null)
            return query.Where(e => e.Id == request.Id);

        return query;
    }
}
