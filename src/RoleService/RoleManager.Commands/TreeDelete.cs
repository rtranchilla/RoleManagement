using AutoMapper;
using MediatR;
using RoleManager.DataPersistence;
using RoleManager.Events;

namespace RoleManager.Commands;

public sealed record TreeDelete(Guid Id) : AggregateRootDelete;
public sealed class TreeDeleteHandler : AggregateRootDeleteHandler<TreeDelete, Tree>
{
    private readonly IPublisher publisher;

    public TreeDeleteHandler(RoleDbContext dbContext, IPublisher publisher) : base(dbContext) => this.publisher = publisher;

    protected override Tree? GetEntity(TreeDelete request, RoleDbContext dbContext) => 
        dbContext.Trees!.FirstOrDefault(e => e.Id == request.Id);

    protected override Task PostSave(TreeDelete request, RoleDbContext dbContext, CancellationToken cancellationToken) =>
        publisher.Publish(new TreeDeleted(request.Id), cancellationToken);
}
