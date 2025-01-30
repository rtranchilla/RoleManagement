using AutoMapper;
using Dapr.Client;
using MediatR;
using RoleManager.Configuration;
using RoleManager.DataPersistence;
using RoleManager.Events;

namespace RoleManager.Commands;

public sealed record RoleCreate(Dto.Role Role) : AggregateRootCreate;
public sealed class RoleCreateHandler(RoleDbContext dbContext, IMapper mapper, DaprClient daprClient, PubSubConfiguration configuration) : AggregateRootCreateHandler<RoleCreate, Role, Dto.Role>(dbContext, mapper)
{
    private List<Node>? nodes;

    protected override async Task PreCreate(RoleCreate request, RoleDbContext dbContext, CancellationToken cancellationToken)
    {
        nodes = new List<Node>();
        foreach (var nodeName in request.Role.Name.Split('_')) 
        {
            var node = dbContext.Nodes!.FirstOrDefault(e => e.Name == nodeName && e.TreeId == request.Role.TreeId);
            if (node == null)
            {
                node = new Node(nodeName, request.Role.TreeId)
                {
                    BaseNode = nodes.Count == 0
                };
                await dbContext.Nodes!.AddAsync(node, cancellationToken);
            }
            nodes.Add(node);
        }        
    }

    protected override Role Map(Dto.Role dto, IMapper mapper) => mapper.Map<Dto.Role, Role>(dto, opt => opt.Items["nodeIds"] = nodes!.Select(e => e.Id).ToArray());
    protected override Dto.Role GetDto(RoleCreate request) => request.Role;
    protected override Task PostMap(RoleCreate request, Role aggregateRoot, RoleDbContext dbContext, CancellationToken cancellationToken) =>
        Task.Run(() =>
        {
            foreach (var id in request.Role.RequiredNodes ?? Array.Empty<Guid>())
            {
                var node = dbContext.Nodes?.FirstOrDefault(e => e.Id == id);
                if (node != null)
                    aggregateRoot.AddRequiredNode(node);
            }
        }, cancellationToken);
    protected override async Task PostSave(Role aggregateRoot, RoleDbContext dbContext, CancellationToken cancellationToken)
    {
        foreach (var node in nodes!)
            await daprClient.PublishEventAsync(
                configuration.Name,
                configuration.Topic.Members.Created,
                new NodeCreated
                {
                    Id = aggregateRoot.Id,
                    Name = node.Name,
                    BaseNode = node.BaseNode
                }, cancellationToken);
    }
}