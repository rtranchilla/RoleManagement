using AutoMapper;
using MediatR;
using RoleManager.DataPersistence;
using RoleManager.Events;
using System.Security.Policy;

namespace RoleManager.Commands;

public sealed record RoleCreate(Dto.Role Role) : AggregateRootCreate;
public sealed class RoleCreateHandler : AggregateRootCreateHandler<RoleCreate, Role, Dto.Role>
{
    public RoleCreateHandler(RoleDbContext dbContext, IMapper mapper, IPublisher publisher) : base(dbContext, mapper) => this.publisher = publisher;

    private List<Node>? nodes;
    private readonly IPublisher publisher;

    protected override async Task PreCreate(RoleCreate request, RoleDbContext dbContext, CancellationToken cancellationToken)
    {
        nodes = new List<Node>();
        foreach (var nodeName in request.Role.Name.Split('_')) 
        {
            var node = dbContext.Nodes!.FirstOrDefault(e => e.Name == nodeName);
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
    protected override async Task PostSave(Role aggregateRoot, RoleDbContext dbContext, CancellationToken cancellationToken)
    {
        foreach (var node in nodes!)
            await publisher.Publish(new NodeCreated(node), cancellationToken);
    }
}