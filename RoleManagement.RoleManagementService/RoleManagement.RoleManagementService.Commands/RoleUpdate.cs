//using AutoMapper;
//using RoleManagement.RoleManagementService.DataPersistence;
//namespace RoleManagement.RoleManagementService.Commands;
//public sealed record RoleUpdate(Dto.Role Role) : AggregateRootUpdate;
//public sealed class RoleUpdateHandler : AggregateRootUpdateHandler<RoleUpdate, Role, Dto.Role>
//{
//    public RoleUpdateHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

//    protected override Role? GetAggregateRoot(RoleUpdate request, RoleDbContext dbContext) =>
//        dbContext.Roles!.FirstOrDefault(e => e.Id == request.Role.Id);

//    protected override Dto.Role GetDto(RoleUpdate request) => request.Role;
//}