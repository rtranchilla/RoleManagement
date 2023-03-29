using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleConfiguration.Commands;
using RoleConfiguration.Queries;

namespace RoleConfiguration.WebService.Controllers;

public sealed class SourceController : SenderControllerBase
{
    public SourceController(ISender sender) : base(sender) { }

    [HttpGet]
    public async Task<ActionResult<string[]>> Get() =>
        await SendQuery(new SourceQuery());

    [HttpPost("{source}")]
    public async Task<IActionResult> Update(string source) =>
        await SendCommand(new SourceCreate(source));

    [HttpDelete("{source}")]
    public async Task<IActionResult> Delete(string source) =>
        await SendCommand(new SourceDelete(source));
}
