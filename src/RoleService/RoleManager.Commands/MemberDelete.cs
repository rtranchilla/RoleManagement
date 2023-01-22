using MediatR;
using RoleManager.DataPersistence;
using RoleManager.Events;

namespace RoleManager.Commands;

public sealed record MemberDelete(Guid Id) : AggregateRootDelete;
public sealed class MemberDeleteHandler : AggregateRootDeleteHandler<MemberDelete, Member>
{
    private readonly IPublisher publisher;

    public MemberDeleteHandler(RoleDbContext dbContext, IPublisher publisher) : base(dbContext) => this.publisher = publisher;

    protected override Member? GetEntity(MemberDelete request, RoleDbContext dbContext) => 
        dbContext.Members!.FirstOrDefault(e => e.Id == request.Id);
    protected override Task PostSave(MemberDelete request, RoleDbContext dbContext, CancellationToken cancellationToken) =>
        publisher.Publish(new MemberDeleted(request.Id), cancellationToken);
}
