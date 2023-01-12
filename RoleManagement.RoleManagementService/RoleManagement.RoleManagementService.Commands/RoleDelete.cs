//using AutoMapper;
//using RoleManagement.RoleManagementService.DataPersistence;


//namespace RoleManagement.RoleManagementService.Commands;

//public sealed record RoleDelete(Guid Id) : AggregateRootDelete;
//public sealed class RoleDeleteHandler : AggregateRootDeleteHandler<RoleDelete, Role>
//{
//    public RoleDeleteHandler(RoleDbContext dbContext) : base(dbContext) { }

//    protected override Role? GetEntity(RoleDelete request, RoleDbContext dbContext)
//    {
//        return dbContext.Roles!.FirstOrDefault(e => e.Id == request.Id);
//    }
//}
