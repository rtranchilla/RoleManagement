using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleManager.Validation
{
    public static class MemberRoleUpdate
    {
        public static bool IsValid(Role newRole, IEnumerable<Guid> memberNodeIds) => 
            newRole.RequiredNodes.All(rn => memberNodeIds.Contains(rn.NodeId));

        public static bool IsValid(Role currentRole, Role newRole, IEnumerable<Role> fullRoleTree, IEnumerable<Guid> memberNodeIds)
        {
            var allRoles = fullRoleTree.ToList();
            allRoles.Add(currentRole);
            allRoles.Add(newRole);

            if (!AreInSameTree(allRoles))
                throw new ArgumentException("Supplied roles must be from the same tree.");

            if (!newRole.RequiredNodes.All(rn => memberNodeIds.Contains(rn.NodeId)))
                return false;

            if (newRole.IsChild(currentRole))
                return true;

            var roleTree = fullRoleTree.ToArray();

            if (currentRole.IsChild(newRole))
                return currentRole.IsReversable(newRole, roleTree);

            //TODO: Need to be able to navigate within tree with the same base node

            return currentRole.IsRebaseable(roleTree);
        }

        private static bool AreInSameTree(IList<Role> roles)
        {
            var firstId = roles[0].TreeId;
            foreach (var role in roles)
                if (firstId != role.TreeId)
                    return false;

            return true;
        }

        private static bool IsChild(this Role childRole, Role parentRole)
        {
            if (parentRole.Nodes.Count >= childRole.Nodes.Count)
                return false;

            for (var i = 0; parentRole.Nodes.Count > i; i++)
                if (parentRole.Nodes[i].NodeId != childRole.Nodes[i].NodeId)
                    return false;

            return true;
        }

        private static bool IsReversable(this Role currentRole, Role newRole, Role[] allRoles)
        {
            if (!currentRole.Reversible)
                return false;

            foreach (var role in allRoles)
                if (currentRole.IsChild(role) && role.IsChild(newRole) && !role.Reversible)
                    return false;

            return true;
        }

        private static bool IsRebaseable(this Role currentRole, Role[] allRoles)
        {
            if (!currentRole.Reversible)
                return false;

            foreach (var role in allRoles)
                if (currentRole.IsChild(role) && !role.Reversible)
                    return false;

            return true;
        }
    }
}
