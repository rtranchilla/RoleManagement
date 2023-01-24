using AutoMapper;
using RoleManager.DataPersistence;

namespace RoleManager.Queries;

public sealed record TreeQuery : AggregateRootQuery<Dto.Tree>
{
    public TreeQuery() { }
    public TreeQuery(Guid id) => Id = id;
    public TreeQuery(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));        

        Name = name;
    }

    public Guid? Id { get; }
    public string? Name { get; }
}

public sealed class TreeQueryHandler : AggregateRootQueryHandler<TreeQuery, Tree, Dto.Tree>
{
    public TreeQueryHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

    protected override IQueryable<Tree> QueryEntities(TreeQuery request, RoleDbContext dbContext)
    {
        var query = dbContext.Trees!.IncludeSubordinate();

        if (request.Id != null)
            return query.Where(e => e.Id == request.Id);
        if (request.Name != null)
            return query.Where(e => e.Name == request.Name);

        return query;  
    }
}
