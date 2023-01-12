//using AutoMapper;
//using RoleManagement.RoleManagementService.DataPersistence;
//namespace RoleManagement.RoleManagementService.Commands;

//public sealed record RoleCreate(Dto.Role Role) : AggregateRootCreate;
//public sealed class RoleCreateHandler : AggregateRootCreateHandler<RoleCreate, Role, Dto.Role>
//{
//    public RoleCreateHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

//    protected override Dto.Role GetDto(RoleCreate request) => request.Role;
//}