using AutoMapper;
using RoleManager.DataPersistence;
namespace RoleManager.Commands;
public sealed record MemberUpdate(Dto.Member Member) : AggregateRootUpdate;
public sealed class MemberUpdateHandler : AggregateRootUpdateHandler<MemberUpdate, Member, Dto.Member>
{
    public MemberUpdateHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

    protected override Member? GetEntity(MemberUpdate request, RoleDbContext dbContext) => 
        dbContext.Members!.FirstOrDefault(e => e.Id == request.Member.Id);

    protected override Dto.Member GetDto(MemberUpdate request) => request.Member;
}