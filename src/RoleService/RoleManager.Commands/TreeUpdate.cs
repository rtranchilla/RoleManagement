using AutoMapper;
using MediatR;
using RoleManager.DataPersistence;
using System.Security.Policy;

namespace RoleManager.Commands;

public sealed record TreeUpdate(Dto.TreeUpdate Tree) : AggregateRootUpdate;

public sealed class TreeUpdateHandler : AggregateRootUpdateHandler<TreeUpdate, Tree, Dto.TreeUpdate>
{
    public TreeUpdateHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

    protected override Dto.TreeUpdate GetDto(TreeUpdate request) => request.Tree;

    protected override Tree? GetEntity(TreeUpdate request, RoleDbContext dbContext) => dbContext.Trees?.FirstOrDefault(e => e.Id == request.Tree.Id);
    protected override Task PostMap(TreeUpdate request, Tree aggregateRoot, RoleDbContext dbContext, CancellationToken cancellationToken) =>
        Task.Run(() =>
        {
            foreach (var id in request.Tree.RequiredNodes ?? Array.Empty<Guid>())
            {
                if (aggregateRoot.RequiredNodes.Any(e => e.NodeId == id))
                    continue;

                var node = dbContext.Nodes?.FirstOrDefault(e => e.Id == id);
                if (node != null)
                    aggregateRoot.AddRequiredNode(node);
            }
            if (request.Tree.RequiredNodes?.Any() ?? false)
                foreach (var reqNode in aggregateRoot.RequiredNodes)
                    if (!request.Tree.RequiredNodes.Contains(reqNode.NodeId))
                        aggregateRoot.RemoveRequiredNode(reqNode.NodeId);
        }, cancellationToken);
}
