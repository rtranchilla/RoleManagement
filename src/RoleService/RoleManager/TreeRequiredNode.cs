namespace RoleManager;

public sealed class TreeRequiredNode : AssociatorEntity<TreeRequiredNode>
{
    public TreeRequiredNode(Guid treeId, Guid nodeId) : base(rn => rn.TreeId, rn => rn.NodeId) 
    {
        TreeId = treeId;
        NodeId = nodeId;
    }

    public Guid TreeId { get; private set; }
    public Guid NodeId { get; private set; }
    public Node? Node { get; private set; }
}
