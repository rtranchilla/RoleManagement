﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManagement.RoleManagementService.Commands;
using RoleManagement.RoleManagementService.Queries;

namespace RoleManagement.RoleManagementService.Web.Controllers;

public class RoleController : AggregateRootCreateDeleteController<Dto.Role, RoleCreate, RoleQuery, RoleDelete>
{
    public RoleController(IMediator mediator) : base(mediator) { }

    protected override RoleCreate GetCreateCommand(Dto.Role dto) => new(dto);
    protected override RoleDelete GetDeleteCommand(Guid id) => new(id);
    protected override RoleQuery GetReadQuery(Guid id) => new(id);

    [HttpPut]
    public Task<IActionResult> Update(Dto.Role role) => SendCommand(new RoleUpdate(role));
}
