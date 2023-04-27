using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleConfiguration.Commands;
using RoleConfiguration.Dto;
using RoleConfiguration.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace RoleConfiguration.WebService.Controllers;

[ApiController]
[Route("File/[controller]")]
public sealed class RoleTreeController : SenderControllerBase
{
    public RoleTreeController(ISender sender) : base(sender) { }

    [HttpGet()]
    [SwaggerOperation(Summary = "Generates the role tree yaml file content for specified roles.",
        Description = "Generates the role tree yaml file content for specified roles. Takes role and tree names.")]
    public async Task<ActionResult<string>> Get([FromQuery]params RoleQuery[] roles) => 
        await SendQuery(new RoleTreeFileQuery(roles.Select(r => (r.Name, r.Tree)).ToArray()));

    [HttpGet("ByTree")]
    [SwaggerOperation(Summary = "Generates the role tree yaml file content for specified tree.",
        Description = "Generates the role tree yaml file content for specified tree.")]
    public async Task<ActionResult<string>> Get(string tree) => 
        await SendQuery(new RoleTreeFileQuery(tree));

    [HttpPut("{source}")]
    public async Task<IActionResult> Update(string source, ContentUpdate content) =>
        await SendCommand(new RoleTreeFileUpdate(source, content.Path, content.Content));
}
