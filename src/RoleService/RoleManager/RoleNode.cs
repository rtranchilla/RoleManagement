using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleManager;

public sealed class RoleNode : AssociatorEntity<RoleNode>
{
    public RoleNode(Guid roleId, Guid nodeId, int order) : base(rn => rn.RoleId, rn => rn.NodeId) 
    {
        RoleId = roleId;
        NodeId = nodeId;
        Order = order;
    }

    public Guid RoleId { get; private set; }
    public Guid NodeId { get; private set; }
    public Node? Node { get; private set; }
    public int Order { get; private set; }
}
