using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManagement.RoleManagementService.Commands;
using RoleManagement.RoleManagementService.Queries;

namespace RoleManagement.RoleManagementService.Web.Controllers;

public class MemberController : AggregateRootCreateDeleteController<Dto.Member, MemberCreate, MemberQuery, MemberDelete>
{
    public MemberController(IMediator mediator) : base(mediator) { }

    protected override MemberCreate GetCreateCommand(Dto.Member dto) => new(dto);
    protected override MemberDelete GetDeleteCommand(Guid id) => new(id);
    protected override MemberQuery GetReadQuery(Guid id) => new(id);

    [HttpGet("ByUniqueName")]
    public Task<ActionResult<IEnumerable<Dto.Member>>> Get(string uniqueName) => SendQuery(new MemberQuery(uniqueName));

    [HttpPut]
    public Task<IActionResult> Update(Dto.Member member) => SendCommand(new MemberUpdate(member));
}
