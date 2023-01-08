using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManagement.RoleManagementService.Commands;
using RoleManagement.RoleManagementService.Dto;
//using RoleManagement.RoleManagementService.Queries;

namespace RoleManagement.RoleManagementService.Web.Controllers;

public abstract class AggregateRootController<TDto, TCreate, TRead, TUpdate, TDelete> : MediatorControllerBase
    where TCreate : AggregateRootCreate
    //where TRead : AggregateRootQuery<TDto>
    where TUpdate : AggregateRootUpdate
    where TDelete : AggregateRootDelete
{
    public AggregateRootController(IMediator mediator) : base(mediator) { }

    [HttpPost]
    public Task<IActionResult> Create(TDto dto) => SendCommand(GetCreateCommand(dto));
    protected abstract TCreate GetCreateCommand(TDto dto);

    //[HttpGet]
    //public Task<ActionResult<IEnumerable<TDto>>> Get(Guid id) => SendQuery(GetReadQuery(id));
    //protected abstract TRead GetReadQuery(Guid id);

    [HttpPut]
    public Task<IActionResult> Update(TDto dto) => SendCommand(GetUpdateCommand(dto));
    protected abstract TUpdate GetUpdateCommand(TDto dto);

    [HttpDelete]
    public Task<IActionResult> Delete(Guid id) => SendCommand(GetDeleteCommand(id));
    protected abstract TDelete GetDeleteCommand(Guid id);
}