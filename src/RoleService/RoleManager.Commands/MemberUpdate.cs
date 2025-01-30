using AutoMapper;
using Dapr.Client;
using MediatR;
using RoleManager.Configuration;
using RoleManager.DataPersistence;
using RoleManager.Events;

namespace RoleManager.Commands;
public sealed record MemberUpdate(Dto.MemberUpdate Member) : AggregateRootUpdate;
public sealed class MemberUpdateHandler(RoleDbContext dbContext, IMapper mapper) : AggregateRootUpdateHandler<MemberUpdate, Member, Dto.MemberUpdate>(dbContext, mapper)
{
    protected override Member? GetEntity(MemberUpdate request, RoleDbContext dbContext) => 
        dbContext.Members!.FirstOrDefault(e => e.Id == request.Member.Id);

    protected override Dto.MemberUpdate GetDto(MemberUpdate request) => request.Member;
}