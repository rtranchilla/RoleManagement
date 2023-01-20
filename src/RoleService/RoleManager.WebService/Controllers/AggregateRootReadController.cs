using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManager.Commands;
using RoleManager.Dto;
using RoleManager.Queries;

namespace RoleManager.Web.Controllers;

public abstract class AggregateRootReadController<TDto, TRead> : MediatorControllerBase
    where TRead : AggregateRootQuery<TDto>, new()
{
    public AggregateRootReadController(IMediator mediator) : base(mediator) { }

    [HttpGet("ById")]
    public Task<ActionResult<IEnumerable<TDto>>> Get(Guid id) => SendQuery(GetReadQuery(id));
    protected abstract TRead GetReadQuery(Guid id);

    [HttpGet]
    public Task<ActionResult<IEnumerable<TDto>>> Get() => SendQuery(new TRead());
}