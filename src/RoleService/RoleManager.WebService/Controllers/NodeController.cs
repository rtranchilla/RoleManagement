using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManager.Commands;
using RoleManager.Queries;

namespace RoleManager.WebService.Controllers;

public class NodeController : SenderControllerBase
{
    public NodeController(ISender sender) : base(sender) { }

    [HttpGet]
    public Task<ActionResult<IEnumerable<Dto.Node>>> Get() => SendQuery(new NodeQuery());

    [HttpGet("{id}")]
    public Task<ActionResult<IEnumerable<Dto.Node>>> Get(Guid id) => SendQuery(new NodeQuery(id));

    [HttpGet("ByName/{name}/{treeId}")]
    public Task<ActionResult<IEnumerable<Dto.Node>>> Get(string name, Guid? treeId = null) => SendQuery(new NodeQuery(name) { TreeId = treeId });

    [HttpGet("ByRole/{roleId}/{treeId}")]
    public Task<ActionResult<IEnumerable<Dto.Node>>> GetByRole(Guid roleId, Guid? treeId = null) => SendQuery(new NodeQuery { RoleId = roleId, TreeId = treeId });
}
