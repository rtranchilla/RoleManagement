using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManager.Commands;
using RoleManager.Queries;

namespace RoleManager.WebService.Controllers;

public sealed class MemberController(ISender sender) : SenderControllerBase(sender)
{
    [HttpPost]
    public Task<IActionResult> Create(Dto.Member dto) => SendCommand(new MemberCreate(dto));

    [HttpGet]
    public Task<ActionResult<IEnumerable<Dto.Member>>> Get() => SendQuery(new MemberQuery());

    [HttpGet("{id}")]
    public Task<ActionResult<IEnumerable<Dto.Member>>> Get(Guid id) => SendQuery(new MemberQuery(id));

    [HttpGet("ByUniqueName/{uniqueName}")]
    public Task<ActionResult<IEnumerable<Dto.Member>>> Get(string uniqueName) => SendQuery(new MemberQuery(uniqueName));

    [HttpGet("ByRole/{roleId}")]
    public Task<ActionResult<IEnumerable<Dto.Member>>> GetByRole(Guid roleId) => SendQuery(new MemberQuery { RoleId = roleId });

    [HttpPut]
    public Task<IActionResult> Update(Dto.MemberUpdate member) => SendCommand(new MemberUpdate(member));

    [HttpDelete("{id}")]
    public Task<IActionResult> Delete(Guid id) => SendCommand(new MemberDelete(id));
}
