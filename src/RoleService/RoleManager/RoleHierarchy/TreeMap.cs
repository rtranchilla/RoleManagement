using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace RoleManager.RoleHierarchy;

public sealed class TreeMap : IReadOnlyDictionary<Guid, RoleNodeRelation>
{
    internal TreeMap(Tree tree, Role[] roles)
    {
        Tree = tree;

        var roleNodeRelations = new Dictionary<Guid, RoleNodeRelation>();
        foreach (var role in roles)
        {
            if (!roleNodeRelations.TryGetValue(role.Nodes[0].NodeId, out var currentRelation))
            {
                currentRelation = new RoleNodeRelation();
                roleNodeRelations[role.Nodes[0].NodeId] = currentRelation;
            }
            int i;
            for (i = 1; i < role.Nodes.Count; i++)
            {
                var nodeId = role.Nodes[i].NodeId;
                if (currentRelation.Relations.ContainsKey(nodeId))
                    currentRelation = currentRelation.Relations[nodeId];
                else
                {
                    var newRelation = new RoleNodeRelation(currentRelation);
                    currentRelation.Relations.Add(nodeId, newRelation);
                    currentRelation = newRelation;
                }
            }
            currentRelation.Role = role;
        }


        foreach (var relation in roleNodeRelations)
            relation.Value.FinalizeRelations();

        baseNodeRelations = roleNodeRelations.ToImmutableDictionary();
    }

    public static TreeMap Build(IEnumerable<Role> roles)
    {
        var tree = roles.First().Tree;
        if (tree is null)
            throw new ArgumentNullException(nameof(tree), "The tree navigation property must be loaded to build a tree map.");

        return new TreeMap(tree, roles.Where(e => e.TreeId == tree.Id).ToArray());
    }

    public bool IsAssignable(Role role, IEnumerable<Guid> memberNodeIds) => IsAssignable(role, memberNodeIds.ToArray());
    public bool IsAssignable(Role role, Guid[] memberNodeIds)
    {
        if (role.TreeId != Tree.Id)
            return false;

        if (baseNodeRelations.ContainsKey(role.Nodes[0].NodeId))
            return GetRelation(role)!.IsAssignable(memberNodeIds);

        return false;
    }

    public bool IsTraversable(Role startRole, Role endRole, IEnumerable<Guid> memberNodeIds) => IsTraversable(startRole, endRole, memberNodeIds.ToArray());
    public bool IsTraversable(Role startRole, Role endRole, Guid[] memberNodeIds)
    {
        if (startRole.TreeId != Tree.Id || endRole.TreeId != Tree.Id)
            return false;

        if (baseNodeRelations.ContainsKey(startRole.Nodes[0].NodeId))
            return GetRelation(startRole)!.CanTraverseTo(endRole, memberNodeIds);

        return false;
    }

    private RoleNodeRelation? GetRelation(Role role)
    {
        IReadOnlyDictionary<Guid, RoleNodeRelation> result = baseNodeRelations;
        foreach (var node in role.Nodes)
            result = result[node.NodeId];

        return (RoleNodeRelation)result;
    }

    public Tree Tree { get; }

    public Role[] GetTraversable(Guid[] memberNodeIds)
    {
        var result = new List<Role>();
        foreach (var relation in baseNodeRelations.Values)
            if (relation.MeetsRequirements(memberNodeIds))
            {
                if (relation.Role != null)
                    result.Add(relation.Role);

                result.AddRange(relation.GetTraversableChildRoles(memberNodeIds));
            }

        return result.ToArray();
    }

    public Role[] GetTraversableFrom(Role startRole, Guid[] memberNodeIds)
    {
        if (startRole.TreeId != Tree.Id)
            throw new ArgumentException("Role specified does not fall within this tree.", nameof(startRole));
        var targetRelation = GetRelation(startRole);
        if (targetRelation?.Role == null)
            throw new ArgumentException("Role specified not found within this tree.", nameof(startRole));

        if (!targetRelation.MeetsRequirements(memberNodeIds))
            return Array.Empty<Role>();

        var result = new List<Role>();
        result.AddRange(targetRelation.GetTraversableFromBase(memberNodeIds));
        var targetBaseNodeId = targetRelation.Role.Nodes[0].NodeId;
        if (baseNodeRelations[targetBaseNodeId].Role?.Reversible ?? true && targetRelation.CanTraverseFromBase(memberNodeIds))
        {
            var baseNodesKeys = baseNodeRelations.Keys.Where(e => e != targetBaseNodeId);
            foreach (var relationId in baseNodesKeys)
            {
                var relation = baseNodeRelations[relationId];
                if (relation.MeetsRequirements(memberNodeIds))
                {
                    if (relation.Role != null)
                        result.Add(relation.Role);

                    result.AddRange(relation.GetTraversableChildRoles(memberNodeIds));
                }
            }
        }

        return result.ToArray();
    }

    private readonly IReadOnlyDictionary<Guid, RoleNodeRelation> baseNodeRelations;

    #region Dictionary Methods
    public RoleNodeRelation this[Guid key] => baseNodeRelations![key];
    public IEnumerable<Guid> Keys => baseNodeRelations!.Keys;
    public IEnumerable<RoleNodeRelation> Values => baseNodeRelations!.Values;
    public int Count => baseNodeRelations!.Count;
    public bool ContainsKey(Guid key) => baseNodeRelations!.ContainsKey(key);
    public IEnumerator<KeyValuePair<Guid, RoleNodeRelation>> GetEnumerator() => baseNodeRelations!.GetEnumerator();
    public bool TryGetValue(Guid key, [MaybeNullWhen(false)] out RoleNodeRelation value) => baseNodeRelations!.TryGetValue(key, out value);
    IEnumerator IEnumerable.GetEnumerator() => baseNodeRelations!.GetEnumerator();
    #endregion
}
