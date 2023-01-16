using AutoMapper;
using RoleManagement.RoleManagementService.DataPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleManagement.RoleManagementService.Commands;

public sealed record MemberDelete(Guid Id) : AggregateRootDelete;
public sealed class MemberDeleteHandler : AggregateRootDeleteHandler<MemberDelete, Member>
{
    public MemberDeleteHandler(RoleDbContext dbContext) : base(dbContext) { }

    protected override Member? GetEntity(MemberDelete request, RoleDbContext dbContext) => 
        dbContext.Members!.FirstOrDefault(e => e.Id == request.Id);
}
