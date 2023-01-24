namespace RoleManager;

public sealed class RoleRequiredNode : AssociatorEntity<RoleRequiredNode>
{
    public RoleRequiredNode(Guid roleId, Guid nodeId) : base(rn => rn.RoleId, rn => rn.NodeId) 
    {
        RoleId = roleId;
        NodeId = nodeId;
    }

    public Guid RoleId { get; private set; }
    public Guid NodeId { get; private set; }
    public Node? Node { get; private set; }
}
