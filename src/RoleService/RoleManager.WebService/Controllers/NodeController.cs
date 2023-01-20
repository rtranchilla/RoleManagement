using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManager.Commands;
using RoleManager.Queries;

namespace RoleManager.Web.Controllers;

public class NodeController : AggregateRootReadController<Dto.Node, NodeQuery>
{
    public NodeController(IMediator mediator) : base(mediator) { }

    protected override NodeQuery GetReadQuery(Guid id) => new(id);

    [HttpGet("ByName")]
    public Task<ActionResult<IEnumerable<Dto.Node>>> Get(string name) => SendQuery(new NodeQuery(name));
}
