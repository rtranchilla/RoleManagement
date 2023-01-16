using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoleManagement.RoleManagementService.DataPersistence;


namespace RoleManagement.RoleManagementService.Commands;

public sealed record RoleDelete(Guid Id) : AggregateRootDelete;
public sealed class RoleDeleteHandler : AggregateRootDeleteHandler<RoleDelete, Role>
{
    public RoleDeleteHandler(RoleDbContext dbContext) : base(dbContext) { }

    protected override Role? GetEntity(RoleDelete request, RoleDbContext dbContext) => 
        dbContext.Roles!.FirstOrDefault(e => e.Id == request.Id);

    protected override async Task PostSave(RoleDelete request, RoleDbContext dbContext) => 
        await dbContext.Database.ExecuteSqlInterpolatedAsync($"Delete from [dbo].[Nodes] where Id not in (Select NodeId from [dbo].[RoleNodes])");
}
