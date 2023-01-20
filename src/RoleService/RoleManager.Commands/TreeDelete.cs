using AutoMapper;
using RoleManager.DataPersistence;


namespace RoleManager.Commands;

public sealed record TreeDelete(Guid Id) : AggregateRootDelete;
public sealed class TreeDeleteHandler : AggregateRootDeleteHandler<TreeDelete, Tree>
{
    public TreeDeleteHandler(RoleDbContext dbContext) : base(dbContext) { }

    protected override Tree? GetEntity(TreeDelete request, RoleDbContext dbContext) => 
        dbContext.Trees!.FirstOrDefault(e => e.Id == request.Id);
}
