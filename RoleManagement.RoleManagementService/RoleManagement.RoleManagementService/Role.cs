namespace RoleManagement.RoleManagementService;

public sealed class Role : EntityWithId
{
    public Role(Guid treeId, params Node[] nodes) : this(Guid.NewGuid(), treeId, nodes) { }
    public Role(Guid id, Guid treeId, params Node[] nodes) : base(id)
    {

        if (nodes.Length < 1)
            throw new ArgumentException($"'{nameof(nodes)}' cannot contain less than 1 members", nameof(nodes));
        Nodes = nodes.ToList();
        TreeId = treeId;
    }

    public bool Reversible { get; set; }
    public IList<Node> Nodes { get; set; }
    public Tree? Tree { get; private set; }
    public Guid TreeId { get; private set; }

}
