﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManagement.RoleManagementService.Commands;
using RoleManagement.RoleManagementService.Queries;

namespace RoleManagement.RoleManagementService.Web.Controllers;

public class TreeController : AggregateRootCreateDeleteController<Dto.Tree, TreeCreate, TreeQuery, TreeDelete>
{
    public TreeController(IMediator mediator) : base(mediator) { }

    protected override TreeCreate GetCreateCommand(Dto.Tree dto) => new(dto);
    protected override TreeDelete GetDeleteCommand(Guid id) => new(id);
    protected override TreeQuery GetReadQuery(Guid id) => new(id);

    [HttpGet("ByName")]
    public Task<ActionResult<IEnumerable<Dto.Tree>>> Get(string name) => SendQuery(new TreeQuery(name));
}