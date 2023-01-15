namespace RoleManagement.RoleManagementService;

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
            throw new ArgumentException($"'{nameof(nodeIds)}' cannot contain less than 1 member(s)", nameof(nodeIds));
        var roleNodes= new List<RoleNode>();
        for (int i = 0; i < nodeIds.Length; i++)
            roleNodes.Add(new RoleNode(id, nodeIds[i], i));
        Nodes = roleNodes;
        TreeId = treeId;
    }

    public bool Reversible { get; set; }
    public IReadOnlyList<RoleNode> Nodes { get; private set; }
    public Tree? Tree { get; private set; }
    public Guid TreeId { get; private set; }

}
