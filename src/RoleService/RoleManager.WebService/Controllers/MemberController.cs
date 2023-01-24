using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManager.Commands;
using RoleManager.Queries;

namespace RoleManager.Web.Controllers;

public sealed class MemberController : SenderControllerBase
{
    public MemberController(ISender sender) : base(sender) { }

    [HttpPost]
    public Task<IActionResult> Create(Dto.Member dto) => SendCommand(new MemberCreate(dto));

    [HttpGet]
    public Task<ActionResult<IEnumerable<Dto.Member>>> Get() => SendQuery(new MemberQuery());

    [HttpGet("ById")]
    public Task<ActionResult<IEnumerable<Dto.Member>>> Get(Guid id) => SendQuery(new MemberQuery(id));

    [HttpGet("ByUniqueName")]
    public Task<ActionResult<IEnumerable<Dto.Member>>> Get(string uniqueName) => SendQuery(new MemberQuery(uniqueName));

    [HttpGet("ByRole")]
    public Task<ActionResult<IEnumerable<Dto.Member>>> GetByRole(Guid roleId) => SendQuery(new MemberQuery { RoleId = roleId });

    [HttpPut]
    public Task<IActionResult> Update(Dto.Member member) => SendCommand(new MemberUpdate(member));

    [HttpDelete]
    public Task<IActionResult> Delete(Guid id) => SendCommand(new MemberDelete(id));
}
