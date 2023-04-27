using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleConfiguration.Commands;
using RoleConfiguration.Dto;
using RoleConfiguration.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace RoleConfiguration.WebService.Controllers;

[ApiController]
[Route("File/[controller]")]
public class MemberController : SenderControllerBase
{
    public MemberController(ISender sender) : base(sender) { }
    /// <summary>
    /// Generates the member yaml file content for specified members.
    /// </summary>
    /// <param name="members">Takes member names.</param>
    /// <returns>member yaml content</returns>
    [HttpGet()]
    [SwaggerOperation(Summary = "Generates the member yaml file content for specified members.",
        Description = "Generates the member yaml file content for specified members. Takes member names.")]
    public async Task<ActionResult<string>> Get([FromQuery] params string[] members) =>
        await SendQuery(new MemberFileQuery(members));

    /// <summary>
    /// Generates the member yaml file content for members of roles specified.
    /// </summary>
    /// <param name="roles">
    /// Takes tree and role names separated by an underscore. 
    /// <example>
    /// Example: TreeName_Role_Name
    /// </example>
    /// </param>
    /// <returns>member yaml content</returns>
    [HttpGet("ByRole")]
    [SwaggerOperation(Summary = "Generates the member yaml file content for members of specified roles.",
        Description = "Generates the member yaml file content for members of roles specified. " +
                      "Takes tree and role names separated by an underscore. E.g. TreeName_Role_Name")]
    public async Task<ActionResult<string>> GetByRole([FromQuery] params string[] roles) =>
        await SendQuery(new MemberFileQuery(roles.Where(e => e.Contains('_')).Select(r =>
        {
            var tree = r.Split('_')[0];
            return (r.Substring(tree.Length + 1), tree);
        }).ToArray()));

    [HttpPut("{source}")]
    public async Task<IActionResult> Update(string source, ContentUpdate content) =>
        await SendCommand(new MemberFileUpdate(source, content.Path, content.Content));
}
