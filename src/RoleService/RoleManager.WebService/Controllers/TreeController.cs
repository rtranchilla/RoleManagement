using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManager.Commands;
using RoleManager.Queries;

namespace RoleManager.WebService.Controllers;

public class TreeController : SenderControllerBase
{
    public TreeController(ISender sender) : base(sender) { }

    [HttpPost]
    public Task<IActionResult> Create(Dto.Tree dto) => SendCommand(new TreeCreate(dto));

    [HttpGet]
    public Task<ActionResult<IEnumerable<Dto.Tree>>> Get() => SendQuery(new TreeQuery());

    [HttpGet("ById")]
    public Task<ActionResult<IEnumerable<Dto.Tree>>> Get(Guid id) => SendQuery(new TreeQuery(id));

    [HttpGet("ByName")]
    public Task<ActionResult<IEnumerable<Dto.Tree>>> Get(string name) => SendQuery(new TreeQuery(name));

    [HttpPut]
    public Task<IActionResult> Update(Dto.TreeUpdate tree) => SendCommand(new TreeUpdate(tree));

    [HttpDelete]
    public Task<IActionResult> Delete(Guid id) => SendCommand(new TreeDelete(id));
}
