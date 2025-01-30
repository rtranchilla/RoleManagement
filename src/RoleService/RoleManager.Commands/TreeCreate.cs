using AutoMapper;
using Dapr.Client;
using MediatR;
using Microsoft.Extensions.Configuration;
using RoleManager.Configuration;
using RoleManager.DataPersistence;
using RoleManager.Dto;
using RoleManager.Events;

namespace RoleManager.Commands;

public sealed record TreeCreate(Dto.Tree Tree) : AggregateRootCreate;
public sealed class TreeCreateHandler(RoleDbContext dbContext, IMapper mapper, DaprClient daprClient, PubSubConfiguration configuration) : AggregateRootCreateHandler<TreeCreate, Tree, Dto.Tree>(dbContext, mapper)
{
    protected override Dto.Tree GetDto(TreeCreate request) => request.Tree;
    protected override async Task PostSave(Tree aggregateRoot, RoleDbContext dbContext, CancellationToken cancellationToken) => 
        await daprClient.PublishEventAsync(
            configuration.Name,
            configuration.Topic.Members.Created,
            new TreeCreated { Id = aggregateRoot.Id, Name = aggregateRoot.Name },
            cancellationToken);

    protected override Task PostMap(TreeCreate request, Tree aggregateRoot, RoleDbContext dbContext, CancellationToken cancellationToken) =>
        Task.Run(() =>
        {
            foreach (var id in request.Tree.RequiredNodes ?? Array.Empty<Guid>())
            {
                var node = dbContext.Nodes?.FirstOrDefault(e => e.Id == id);
                if (node != null)
                    aggregateRoot.AddRequiredNode(node);
            }
        }, cancellationToken);
}