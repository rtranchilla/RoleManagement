using AutoMapper;
using Dapr.Client;
using MediatR;
using Microsoft.Extensions.Configuration;
using RoleManager.Configuration;
using RoleManager.DataPersistence;
using RoleManager.Events;

namespace RoleManager.Commands;

public sealed record TreeDelete(Guid Id) : AggregateRootDelete;
public sealed class TreeDeleteHandler(RoleDbContext dbContext, DaprClient daprClient, PubSubConfiguration configuration) : AggregateRootDeleteHandler<TreeDelete, Tree>(dbContext)
{
    protected override Tree? GetEntity(TreeDelete request, RoleDbContext dbContext) => 
        dbContext.Trees!.FirstOrDefault(e => e.Id == request.Id);

    protected override async Task PostSave(TreeDelete request, RoleDbContext dbContext, CancellationToken cancellationToken) =>
        await daprClient.PublishEventAsync(
            configuration.Name,
            configuration.Topic.Members.Created,
            new TreeDeleted { Id = request.Id },
            cancellationToken);
}
