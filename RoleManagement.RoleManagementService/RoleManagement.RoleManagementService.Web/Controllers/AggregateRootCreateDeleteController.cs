using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManagement.RoleManagementService.Commands;
using RoleManagement.RoleManagementService.Dto;
using RoleManagement.RoleManagementService.Queries;

namespace RoleManagement.RoleManagementService.Web.Controllers;

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