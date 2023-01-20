using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManager.Commands;
using RoleManager.Dto;
using RoleManager.Queries;

namespace RoleManager.Web.Controllers;

public abstract class AggregateRootCreateDeleteController<TDto, TCreate, TRead, TDelete> : AggregateRootReadController<TDto, TRead>
    where TCreate : AggregateRootCreate
    where TRead : AggregateRootQuery<TDto>, new()
    where TDelete : AggregateRootDelete
{
    public AggregateRootCreateDeleteController(IMediator mediator) : base(mediator) { }

    [HttpPost]
    public Task<IActionResult> Create(TDto dto) => SendCommand(GetCreateCommand(dto));
    protected abstract TCreate GetCreateCommand(TDto dto);

    [HttpDelete]
    public Task<IActionResult> Delete(Guid id) => SendCommand(GetDeleteCommand(id));
    protected abstract TDelete GetDeleteCommand(Guid id);
}