using Microsoft.EntityFrameworkCore;
using RoleManager.DataPersistence;

namespace RoleManager.Commands;

internal static class MemberFunctions
{        
    public static Guid[] GetNodeIds(RoleDbContext dbContext, Guid memberId) =>
        dbContext.Members!.Include(e => e.Roles)
                          .ThenInclude(e => e.Role)
                          .ThenInclude(e => e!.Nodes)
                          .First(e => e.Id == memberId)
                          .Roles
                          .SelectMany(e => e.Role!.Nodes)
                          .Select(e => e.NodeId).ToArray();
}
