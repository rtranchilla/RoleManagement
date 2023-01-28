using AutoMapper;
using RoleManager.DataPersistence;

namespace RoleManager.Commands;

public sealed record RoleUpdate(Dto.RoleUpdate Role) : AggregateRootUpdate;
public sealed class RoleUpdateHandler : AggregateRootUpdateHandler<RoleUpdate, Role, Dto.RoleUpdate>
{
    public RoleUpdateHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

    protected override Role? GetEntity(RoleUpdate request, RoleDbContext dbContext) =>
        dbContext.Roles!.FirstOrDefault(e => e.Id == request.Role.Id);

    protected override Dto.RoleUpdate GetDto(RoleUpdate request) => request.Role;
    protected override Task PostMap(RoleUpdate request, Role aggregateRoot, RoleDbContext dbContext, CancellationToken cancellationToken) =>
        Task.Run(() =>
        {
            foreach (var id in request.Role.RequiredNodes ?? Array.Empty<Guid>())
            {
                if (aggregateRoot.RequiredNodes.Any(e => e.NodeId == id))
                    continue;

                var node = dbContext.Nodes?.FirstOrDefault(e => e.Id == id);
                if (node != null)
                    aggregateRoot.AddRequiredNode(node);
            }
            if (request.Role.RequiredNodes?.Any() ?? false)
                foreach (var reqNode in aggregateRoot.RequiredNodes)
                    if (!request.Role.RequiredNodes.Contains(reqNode.NodeId))
                        aggregateRoot.RemoveRequiredNode(reqNode.NodeId);
        }, cancellationToken);
}