using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManager.Commands;
using RoleManager.Queries;

namespace RoleManager.WebService.Controllers;

public class RoleController : SenderControllerBase
{
    public RoleController(ISender sender) : base(sender) { }

    [HttpPost]
    public Task<IActionResult> Create(Dto.Role dto) => SendCommand(new RoleCreate(dto));

    [HttpGet]
    public Task<ActionResult<IEnumerable<Dto.Role>>> Get() => SendQuery(new RoleQuery());

    [HttpGet("{id}")]
    public Task<ActionResult<IEnumerable<Dto.Role>>> Get(Guid id) => SendQuery(new RoleQuery(id));

    [HttpGet("ByName/{name}/{tree}")]
    public Task<ActionResult<IEnumerable<Dto.Role>>> Get(string name, string tree) => SendQuery(new RoleQuery(name, tree));

    [HttpGet("ByTree/{tree}")]
    public Task<ActionResult<IEnumerable<Dto.Role>>> Get(string tree) => SendQuery(new RoleQuery(tree));

    [HttpGet("ByMember/{memberId}")]
    public Task<ActionResult<IEnumerable<Dto.Role>>> GetByMember(Guid memberId) => SendQuery(new MemberRoleQuery(memberId));

    [HttpGet("AvailableForMember/{memberId}")]
    public Task<ActionResult<IEnumerable<Dto.Role>>> GetAvailableForMember(Guid memberId) => SendQuery(new RoleAvailableQuery(memberId));

    [HttpPut]
    public Task<IActionResult> Update(Dto.RoleUpdate role) => SendCommand(new RoleUpdate(role));

    [HttpDelete("{id}")]
    public Task<IActionResult> Delete(Guid id) => SendCommand(new RoleDelete(id));
}
