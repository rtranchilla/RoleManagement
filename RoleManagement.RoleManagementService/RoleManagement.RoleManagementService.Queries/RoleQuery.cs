//using AutoMapper;
//using RoleManagement.RoleManagementService.DataPersistence;

//namespace RoleManagement.RoleManagementService.Queries;

//public sealed record RoleQuery : AggregateRootQuery<Dto.Role>
//{
//    public RoleQuery() { }
//    public RoleQuery(Guid id) => Id = id;
//    public RoleQuery(string name)
//    {
//        if (string.IsNullOrWhiteSpace(name))
//            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));        

//        UniqueName = name;
//    }

//    public Guid? Id { get; }
//    public string? UniqueName { get; }
//}

//public sealed class RoleQueryHandler : AggregateRootQueryHandler<RoleQuery, Role, Dto.Role>
//{
//    public RoleQueryHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

//    protected override IQueryable<Role> QueryEntities(RoleQuery request, RoleDbContext dbContext)
//    {
//        if (request.Id != null)
//            return dbContext.Roles!.Where(e => e.Id == request.Id);
//        if (request.UniqueName != null)
//            return dbContext.Roles!.Where(e => e.Name == request.Name);

//        return dbContext.Roles!;  
//    }
//}
