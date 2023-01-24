using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManager.Commands;
using RoleManager.Queries;

namespace RoleManager.Web.Controllers;

public class NodeController : SenderControllerBase
{
    public NodeController(ISender sender) : base(sender) { }

    [HttpGet]
    public Task<ActionResult<IEnumerable<Dto.Node>>> Get() => SendQuery(new NodeQuery());

    [HttpGet("ById")]
    public Task<ActionResult<IEnumerable<Dto.Node>>> Get(Guid id) => SendQuery(new NodeQuery(id));

    [HttpGet("ByName")]
    public Task<ActionResult<IEnumerable<Dto.Node>>> Get(string name) => SendQuery(new NodeQuery(name));

    [HttpGet("ByRole")]
    public Task<ActionResult<IEnumerable<Dto.Node>>> GetByRole(Guid roleId) => SendQuery(new NodeQuery { RoleId = roleId });
}
