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
        Description = "Generates the role tree yaml file content for specified roles. " +
                      "Takes tree and role names separated by an underscore. E.g. TreeName_Role_Name")]
    public async Task<ActionResult<string>> Get([FromQuery]params string[] role) => 
        await SendQuery(new RoleTreeFileQuery(role.Where(e => e.Contains('_')).Select(r =>
        {
            var tree = r.Split('_')[0];
            return (r.Substring(tree.Length + 1), tree);
        }).ToArray()));

    [HttpGet("ByTree")]
    [SwaggerOperation(Summary = "Generates the role tree yaml file content for specified tree.",
        Description = "Generates the role tree yaml file content for specified tree.")]
    public async Task<ActionResult<string>> Get(string tree) => 
        await SendQuery(new RoleTreeFileQuery(tree));

    [HttpPut("{source}")]
    [SwaggerOperation(Summary = "Updates a role tree yaml file's content.",
        Description = "Updates a role tree yaml file's content for a specified source.")]
    public async Task<IActionResult> Update(string source, ContentUpdate content) =>
        await SendCommand(new RoleTreeFileUpdate(source, content.Path, content.Content));
}
