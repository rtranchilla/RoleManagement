using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleConfiguration.Commands;
using RoleConfiguration.Dto;
using RoleConfiguration.Queries;

namespace RoleConfiguration.WebService.Controllers;

[ApiController]
[Route("File/[controller]")]
public sealed class RoleTreeController : SenderControllerBase
{
    public RoleTreeController(ISender sender) : base(sender) { }

    [HttpGet()]
    public async Task<ActionResult<string>> Get([FromQuery]params string[] members) => 
        await SendQuery(new MemberFileQuery(members));

    [HttpPut("{source}")]
    public async Task<IActionResult> Update(string source, ContentUpdate content) =>
        await SendCommand(new RoleTreeFileUpdate(source, content.Path, content.Content));
}
