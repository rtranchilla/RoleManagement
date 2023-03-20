using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoleManager.DataPersistence;

namespace RoleManager.Queries;

public sealed record NodeQuery : AggregateRootQuery<Dto.Node>
{
    public NodeQuery() { }
    public NodeQuery(Guid id) => Id = id;
    public NodeQuery(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));        

        Name = name;
    }

    public Guid? Id { get; }
    public string? Name { get; }
    public Guid? RoleId { get; set; }
    public Guid? TreeId { get; set; }
}

public sealed class NodeQueryHandler : AggregateRootQueryHandler<NodeQuery, Node, Dto.Node>
{
    public NodeQueryHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

    protected override IQueryable<Node> QueryEntities(NodeQuery request, RoleDbContext dbContext)
    {
        var query = dbContext.Nodes!.IncludeSubordinate();
        if (request.TreeId != null)
            query = query.Where(e => e.TreeId == request.TreeId);
        if (request.Id != null)
            return query.Where(e => e.Id == request.Id);
        if (request.Name != null)
            return query.Where(e => e.Name == request.Name);
        if (request.RoleId != null)
            return dbContext.Roles!.IncludeSubordinate(true)
                                   .Where(e => e.Id == request.RoleId)
                                   .SelectMany(e => e.Nodes)
                                   .Select(e => e.Node!);

        return query;  
    }
}
