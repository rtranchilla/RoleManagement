using System.Collections.Immutable;

namespace RoleManager;

public sealed class Tree : EntityWithId
{
    public Tree(string name) : this(Guid.NewGuid(), name) { }
    public Tree(Guid id, string name) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        Name = name;
    }

    public string Name { get; private set; }

    public void AddRequiredNode(Node node) => AddRequiredNode(node.Id, node.TreeId);
    public void AddRequiredNode(Guid nodeId, Guid nodeTreeId)
    {
        if (nodeTreeId == Id)
            throw new ArgumentException("Node requirement cannot be added within the same tree.");
        if (requiredNodes.Any(e => e.NodeId == nodeId))
            throw new ArgumentException("Required node already exists.");

        requiredNodes.Add(new TreeRequiredNode(Id, nodeId));
    }

    public void RemoveRequiredNode(Node node) => RemoveRequiredNode(node.Id);
    public void RemoveRequiredNode(Guid nodeId)
    {
        var reqNode = requiredNodes.FirstOrDefault(e => e.NodeId == nodeId);

        if (reqNode != null)
            requiredNodes.Remove(reqNode);
    }

    private readonly List<TreeRequiredNode> requiredNodes = new();
    public IReadOnlyList<TreeRequiredNode> RequiredNodes { get => requiredNodes.ToImmutableList(); }
}
