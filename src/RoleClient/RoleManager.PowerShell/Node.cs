namespace RoleManager.PowerShell;

public sealed class Node
{
    public Node(Guid id, string name, bool baseNode, Guid treeId)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        BaseNode = baseNode;
        TreeId = treeId;
    }

    public Guid Id { get; }
    public string Name { get; }
    public bool BaseNode { get; }
    public Guid TreeId { get; }
}