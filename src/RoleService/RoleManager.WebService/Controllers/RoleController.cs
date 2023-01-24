using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManager.Commands;
using RoleManager.Queries;

namespace RoleManager.Web.Controllers;

public class RoleController : SenderControllerBase
{
    public RoleController(ISender sender) : base(sender) { }

    [HttpPost]
    public Task<IActionResult> Create(Dto.Role dto) => SendCommand(new RoleCreate(dto));

    [HttpGet]
    public Task<ActionResult<IEnumerable<Dto.Role>>> Get() => SendQuery(new RoleQuery());

    [HttpGet("ById")]
    public Task<ActionResult<IEnumerable<Dto.Role>>> Get(Guid id) => SendQuery(new RoleQuery(id));

    [HttpGet("ByMember")]
    public Task<ActionResult<IEnumerable<Dto.Role>>> GetByMember(Guid memberId) => SendQuery(new MemberRoleQuery(memberId));

    [HttpPut]
    public Task<IActionResult> Update(Dto.Role role) => SendCommand(new RoleUpdate(role));

    [HttpDelete]
    public Task<IActionResult> Delete(Guid id) => SendCommand(new RoleDelete(id));
}
