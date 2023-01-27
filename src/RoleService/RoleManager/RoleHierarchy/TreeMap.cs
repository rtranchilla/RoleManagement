using System.Collections;
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

        relations = roleNodeRelations.ToImmutableDictionary();
    }

    public static TreeMap Build(IEnumerable<Role> roles)
    {
        var tree = roles.First().Tree;
        if (tree is null)
            throw new ArgumentNullException(nameof(tree), "The tree navigation property must be loaded to build a tree map.");

        return new TreeMap(tree, roles.Where(e => e.TreeId == tree.Id).ToArray());
    }

    public bool IsTraversable(Role start, Role end)
    {
        if (start.TreeId != Tree.Id || end.TreeId != Tree.Id)
            return false;

        if (relations.ContainsKey(start.Nodes[0].NodeId))
            return GetRelation(start)!.CanTraverseTo(end);

        return false;
    }

    private RoleNodeRelation? GetRelation(Role role)
    {
        IReadOnlyDictionary<Guid, RoleNodeRelation> result = relations;
        foreach (var node in role.Nodes)
            result = result[node.NodeId];

        return (RoleNodeRelation)result;
    }

    public Tree Tree { get; }

    private readonly IReadOnlyDictionary<Guid, RoleNodeRelation> relations;

    #region Dictionary Methods
    public RoleNodeRelation this[Guid key] => relations![key];
    public IEnumerable<Guid> Keys => relations!.Keys;
    public IEnumerable<RoleNodeRelation> Values => relations!.Values;
    public int Count => relations!.Count;
    public bool ContainsKey(Guid key) => relations!.ContainsKey(key);
    public IEnumerator<KeyValuePair<Guid, RoleNodeRelation>> GetEnumerator() => relations!.GetEnumerator();
    public bool TryGetValue(Guid key, [MaybeNullWhen(false)] out RoleNodeRelation value) => relations!.TryGetValue(key, out value);
    IEnumerator IEnumerable.GetEnumerator() => relations!.GetEnumerator();
    #endregion
}
