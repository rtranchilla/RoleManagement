using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManagement.RoleManagementService.Commands;
using RoleManagement.RoleManagementService.MapperConfig;
using RoleManagement.RoleManagementService.Queries;

namespace RoleManagement.RoleManagementService.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class MemberController : AggregateRootController<Dto.Member, MemberCreate, MemberQuery, MemberUpdate, MemberDelete>
{
    public MemberController(IMediator mediator) : base(mediator) { }

    protected override MemberCreate GetCreateCommand(Dto.Member dto) => new(dto);
    protected override MemberDelete GetDeleteCommand(Guid id) => new(id);
    protected override MemberUpdate GetUpdateCommand(Dto.Member dto) => new(dto);
    
}
