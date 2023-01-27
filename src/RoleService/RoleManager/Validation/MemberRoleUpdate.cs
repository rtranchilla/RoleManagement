namespace RoleManager.Validation;

public static class MemberRoleUpdate
{
    public static bool HasRequired(Role newRole, IEnumerable<Guid> memberNodeIds) =>
        newRole.RequiredNodes.All(rn => memberNodeIds.Contains(rn.NodeId));
}
