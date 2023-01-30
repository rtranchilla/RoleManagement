using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace RoleManager.RoleHierarchy;

public sealed class RoleNodeRelation : IReadOnlyDictionary<Guid, RoleNodeRelation>
{
    public RoleNodeRelation(RoleNodeRelation? parent = null) => Parent = parent;

    public Role? Role { get; internal set; }
    public RoleNodeRelation? Parent { get; }

    internal Dictionary<Guid, RoleNodeRelation> Relations { get; } = new();
    private IReadOnlyDictionary<Guid, RoleNodeRelation>? finalizedRelations;

    internal void FinalizeRelations()
    {
        finalizedRelations = Relations.ToImmutableDictionary();
        foreach (var relation in finalizedRelations)
            relation.Value.FinalizeRelations();
    }

    internal Role[] GetTraversableFromBase(Guid[] memberNodeIds)
    {
        if (Role?.Reversible ?? true && Parent != null && Parent.MeetsRequirements(memberNodeIds))
            return Parent!.GetTraversableFromBase(memberNodeIds);

        var result = new List<Role>(GetTraversableChildRoles(memberNodeIds));
        if (Role != null)
            result.Add(Role);
        return result.ToArray();
    }

    public Role[] GetTraversableChildRoles(Guid[] memberNodeIds)
    {
        var result = new List<Role>();
        if (MeetsRequirements(memberNodeIds))
            foreach (var relation in Relations.Values)
            {
                if (relation.Role != null)
                    result.Add(relation.Role);

                result.AddRange(relation.GetTraversableChildRoles(memberNodeIds));
            }
        return result.ToArray();
    }

    public bool CanTraverseTo(Role endRole, Guid[] memberNodeIds)
    {
        if (Role == null)
            return Parent?.CanTraverseTo(endRole, memberNodeIds) ?? true;

        if (!MeetsRequirements(memberNodeIds))
            return false;

        if (IsEqualOrChild(endRole))
            return true;

        if (Role.Reversible)
            return Parent!.CanTraverseTo(endRole, memberNodeIds);

        return false;
    }

    public bool CanTraverseFromBase(Guid[] memberNodeIds)
    {
        if (Role == null)
            return Parent?.CanTraverseFromBase(memberNodeIds) ?? true;

        if (!MeetsRequirements(memberNodeIds))
            return false;

        if (Role.Reversible)
            return Parent!.CanTraverseFromBase(memberNodeIds);

        return false;
    }

    public bool IsAssignable(Guid[] memberNodeIds) =>
        Parent?.MeetsRequirements(memberNodeIds) ?? true &&
        MeetsRequirements(memberNodeIds);

    private bool IsEqualOrChild(Role childRole)
    {
        if (Role!.Nodes.Count > childRole.Nodes.Count)
            return false;

        for (var i = 0; Role.Nodes.Count > i; i++)
            if (Role.Nodes[i].NodeId != childRole.Nodes[i].NodeId)
                return false;

        return true;
    }

    internal bool MeetsRequirements(Guid[] memberNodeIds)
    {
        foreach (var node in Role?.RequiredNodes ?? Array.Empty<RoleRequiredNode>())
            if (!memberNodeIds.Contains(node.NodeId))
                return false;

        return true;
    }

    #region Dictionary Methods
    public RoleNodeRelation this[Guid key] => finalizedRelations![key];
    public IEnumerable<Guid> Keys => finalizedRelations!.Keys;
    public IEnumerable<RoleNodeRelation> Values => finalizedRelations!.Values;
    public int Count => finalizedRelations!.Count;
    public bool ContainsKey(Guid key) => finalizedRelations!.ContainsKey(key);
    public IEnumerator<KeyValuePair<Guid, RoleNodeRelation>> GetEnumerator() => finalizedRelations!.GetEnumerator();
    public bool TryGetValue(Guid key, [MaybeNullWhen(false)] out RoleNodeRelation value) => finalizedRelations!.TryGetValue(key, out value);
    IEnumerator IEnumerable.GetEnumerator() => finalizedRelations!.GetEnumerator();
    #endregion
}