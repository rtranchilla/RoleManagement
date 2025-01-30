using Dapr.Client;
using RoleManager.Configuration;
using RoleManager.DataPersistence;
using RoleManager.Events;

namespace RoleManager.Commands;

public sealed record MemberDelete(Guid Id) : AggregateRootDelete;
public sealed class MemberDeleteHandler(RoleDbContext dbContext, DaprClient daprClient, PubSubConfiguration configuration) : AggregateRootDeleteHandler<MemberDelete, Member>(dbContext)
{
    protected override Member? GetEntity(MemberDelete request, RoleDbContext dbContext) => 
        dbContext.Members!.FirstOrDefault(e => e.Id == request.Id);
    protected override async Task PostSave(MemberDelete request, RoleDbContext dbContext, CancellationToken cancellationToken) =>
        await daprClient.PublishEventAsync(
            configuration.Name,
            configuration.Topic.Members.Deleted,
            new MemberDeleted { Id = request.Id }, 
            cancellationToken);
}
