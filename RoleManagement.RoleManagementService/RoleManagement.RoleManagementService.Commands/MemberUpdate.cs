using AutoMapper;
using RoleManagement.RoleManagementService.DataPersistence;
namespace RoleManagement.RoleManagementService.Commands;
public sealed record MemberUpdate(Dto.Member Member) : AggregateRootUpdate;
public sealed class MemberUpdateHandler : AggregateRootUpdateHandler<MemberUpdate, Member, Dto.Member>
{
    public MemberUpdateHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

    protected override Member? GetAggregateRoot(MemberUpdate request, RoleDbContext dbContext)
    {
        return dbContext.Members!.FirstOrDefault(e => e.Id == request.Member.Id);
    }

    protected override Dto.Member GetDto(MemberUpdate request)
    {
        return request.Member;
    }
}