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
}

public sealed class NodeQueryHandler : AggregateRootQueryHandler<NodeQuery, Node, Dto.Node>
{
    public NodeQueryHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

    protected override IQueryable<Node> QueryEntities(NodeQuery request, RoleDbContext dbContext)
    {
        if (request.Id != null)
            return dbContext.Nodes!.Where(e => e.Id == request.Id);
        if (request.Name != null)
            return dbContext.Nodes!.Where(e => e.Name == request.Name);
        if (request.RoleId != null)
            return dbContext.Roles!.Include(e => e.Nodes)
                                   .ThenInclude(e => e.Node)
                                   .Where(e => e.Id == request.RoleId)
                                   .SelectMany(e => e.Nodes)
                                   .Select(e => e.Node!);

        return dbContext.Nodes!;  
    }
}
