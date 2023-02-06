using AutoMapper;
using MediatR;
using RoleManager.DataPersistence;
using RoleManager.Events;

namespace RoleManager.Commands;
public sealed record MemberUpdate(Dto.MemberUpdate Member) : AggregateRootUpdate;
public sealed class MemberUpdateHandler : AggregateRootUpdateHandler<MemberUpdate, Member, Dto.MemberUpdate>
{
    private readonly IPublisher publisher;

    public MemberUpdateHandler(RoleDbContext dbContext, IMapper mapper, IPublisher publisher) : base(dbContext, mapper) => this.publisher = publisher;

    protected override Member? GetEntity(MemberUpdate request, RoleDbContext dbContext) => 
        dbContext.Members!.FirstOrDefault(e => e.Id == request.Member.Id);

    protected override Dto.MemberUpdate GetDto(MemberUpdate request) => request.Member;
    protected override Task PostSave(Member aggregateRoot, RoleDbContext dbContext, CancellationToken cancellationToken) =>
        publisher.Publish(new MemberUpdated(aggregateRoot, MemberFunctions.GetNodeIds(dbContext, aggregateRoot.Id)), cancellationToken);
}