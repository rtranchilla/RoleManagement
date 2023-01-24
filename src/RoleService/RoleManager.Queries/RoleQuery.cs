using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoleManager.DataPersistence;

namespace RoleManager.Queries;

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
        IQueryable<Role> query = dbContext.Roles!.IncludeSubordinate(true);

        if (request.Id != null)
            return query.Where(e => e.Id == request.Id);
        if (request.MemberId != null)
            return dbContext.Members!.IncludeSubordinate(true)
                                     .Where(e => e.Id == request.MemberId)
                                     .SelectMany(e => e.Roles)
                                     .Select(e => e.Role)
                                     .Cast<Role>();

        return query;
    }
}
