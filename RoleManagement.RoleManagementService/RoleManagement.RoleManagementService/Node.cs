namespace RoleManagement.RoleManagementService;

public sealed class Node : EntityWithId
{
    public Node(string name, Guid treeId) : this(Guid.NewGuid(), name, treeId) { }
    public Node(Guid id, string name, Guid treeId) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        Name = name;
        TreeId = treeId;
    }

    public bool BaseNode { get; set; }
    public string Name { get; private set; }
    public Tree? Tree { get; private set; }
    public Guid TreeId { get; private set; }
}
