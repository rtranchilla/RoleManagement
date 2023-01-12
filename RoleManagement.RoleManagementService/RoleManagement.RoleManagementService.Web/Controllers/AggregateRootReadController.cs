using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManagement.RoleManagementService.Commands;
using RoleManagement.RoleManagementService.Dto;
using RoleManagement.RoleManagementService.Queries;

namespace RoleManagement.RoleManagementService.Web.Controllers;

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