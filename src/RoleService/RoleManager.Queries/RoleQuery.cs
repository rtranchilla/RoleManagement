using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoleManager.DataPersistence;

namespace RoleManager.Queries;

public sealed record RoleQuery : AggregateRootQuery<Dto.Role>
{
    public RoleQuery() { }
    public RoleQuery(Guid id) => Id = id;
    public RoleQuery(string tree)
    {
        if (string.IsNullOrWhiteSpace(tree))
            throw new ArgumentException($"'{nameof(tree)}' cannot be null or whitespace.", nameof(tree));

        Tree = tree;
    }
    public RoleQuery(string name, string tree)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        if (string.IsNullOrWhiteSpace(tree))
            throw new ArgumentException($"'{nameof(tree)}' cannot be null or whitespace.", nameof(tree));

        Name = name;
        Tree = tree;
    }

    public Guid? Id { get; }
    public Guid? MemberId { get; set; }
    public string? Name { get; }
    public string? Tree { get; }
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
        if (request.Name != null)
            return query.Where(e => e.Id == RoleDbContext.RoleIdFromName(request.Name, request.Tree!));

        // ToDo: Implement Tree Query

        return query;
    }
}
