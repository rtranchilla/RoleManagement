using System.Collections.Immutable;

namespace RoleManager;

public sealed class Role : EntityWithId
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    // parameterless constructor for Entity Framework
    private Role() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Role(Guid treeId, params Guid[] nodeIds) : this(Guid.NewGuid(), treeId, nodeIds) { }
    public Role(Guid id, Guid treeId, params Guid[] nodeIds) : base(id)
    {
        if (nodeIds.Length < 1)
            throw new ArgumentException($"'{nameof(nodeIds)}' cannot contain less than 1 member", nameof(nodeIds));
        if (nodeIds.GroupBy(x => x).SelectMany(g => g.Skip(1)).Any())
            throw new ArgumentException($"'{nameof(nodeIds)}' cannot contain duplicate members", nameof(nodeIds));
        var roleNodes = new List<RoleNode>();
        for (int i = 0; i < nodeIds.Length; i++)
            roleNodes.Add(new RoleNode(id, nodeIds[i], i));
        Nodes = roleNodes;
        TreeId = treeId;
    }

    public bool Reversible { get; set; }
    public IReadOnlyList<RoleNode> Nodes { get; private set; }
    public Tree? Tree { get; private set; }
    public Guid TreeId { get; private set; }

    public void AddRequiredNode(Node node) => AddRequiredNode(node.Id, node.TreeId);
    public void AddRequiredNode(Guid nodeId, Guid nodeTreeId)
    {
        if (nodeTreeId == TreeId)
            throw new ArgumentException("Node requirement cannot be added within the same tree.");
        if (requiredNodes.Any(e => e.NodeId == nodeId))
            throw new ArgumentException("Required node already exists.");

        requiredNodes.Add(new RoleRequiredNode(Id, nodeId));
    }

    public void RemoveRequiredNode(Node node) => RemoveRequiredNode(node.Id);
    public void RemoveRequiredNode(Guid nodeId)
    {
        var reqNode = requiredNodes.FirstOrDefault(e => e.NodeId == nodeId);

        if (reqNode != null)
            requiredNodes.Remove(reqNode);
    }

    private readonly List<RoleRequiredNode> requiredNodes = new();
    public IReadOnlyList<RoleRequiredNode> RequiredNodes { get => requiredNodes.ToImmutableList(); }
}
