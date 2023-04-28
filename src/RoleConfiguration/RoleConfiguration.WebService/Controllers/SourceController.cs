using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleConfiguration.Commands;
using RoleConfiguration.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace RoleConfiguration.WebService.Controllers;

public sealed class SourceController : SenderControllerBase
{
    public SourceController(ISender sender) : base(sender) { }

    [HttpGet]
    [SwaggerOperation(Summary = "Get sources.",
        Description = "Gets all configured sources.")]
    public async Task<ActionResult<string[]>> Get() =>
        await SendQuery(new SourceQuery());

    [HttpPost("{source}")]
    [SwaggerOperation(Summary = "Create source.",
        Description = "Creates a yaml file source.")]
    public async Task<IActionResult> Create(string source) =>
        await SendCommand(new SourceCreate(source));

    [HttpDelete("{source}")]
    [SwaggerOperation(Summary = "Delete source.",
        Description = "Deletes a yaml file source.")]
    public async Task<IActionResult> Delete(string source) =>
        await SendCommand(new SourceDelete(source));
}
