using AutoMapper;
using RoleManagement.RoleManagementService.DataPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleManagement.RoleManagementService.Commands;

public sealed record MemberCreate(Dto.Member Member) : AggregateRootCreate;
public sealed class MemberCreateHandler : AggregateRootCreateHandler<MemberCreate, Member, Dto.Member>
{
    public MemberCreateHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

    protected override Dto.Member GetDto(MemberCreate request) => request.Member;
}