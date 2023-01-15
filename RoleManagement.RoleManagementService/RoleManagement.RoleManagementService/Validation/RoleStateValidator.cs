using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleManagement.RoleManagementService.Validation
{
    public sealed class RoleValidator : IValidator<Role>
    {
        public bool Validate(Role entity)
        {
            // Verify role has at least one node
            if (entity?.Nodes == null || entity.Nodes.Count < 1)
                return false;

            // Verify first node is a base node
            if (!(entity.Nodes.FirstOrDefault(e => e.Order == 0)?.Node?.BaseNode ?? false))
                return false;

            // Verify remaining nodes are not a base node
            if (entity.Nodes.Where(e => e.Order > 0 && (e.Node?.BaseNode ?? true)).Any())
                return false;


            return true;
        }
    }
}
