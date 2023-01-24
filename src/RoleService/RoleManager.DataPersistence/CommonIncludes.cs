using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RoleManager.DataPersistence
{
    public static class CommonIncludes
    {
        public static IQueryable<Member> IncludeSubordinate(this IQueryable<Member> members, bool includeNavigation = false) => includeNavigation
                ? members.Include(e => e.Roles)
                         .ThenInclude(e => e.Role!.Nodes.OrderBy(rn => rn.Order))
                         .ThenInclude(e => e.Node!.Tree)
                         .Include(e => e.Roles)
                         .ThenInclude(e => e.Role!.RequiredNodes)
                         .ThenInclude(e => e.Node!.Tree)
                         .Include(e => e.Roles)
                         .ThenInclude(e => e.Role!.Tree)
                : members.Include(e => e.Roles);
        public static IQueryable<Role> IncludeSubordinate(this IQueryable<Role> roles, bool includeNavigation = false) => includeNavigation
                ? roles.Include(e => e.Nodes.OrderBy(rn => rn.Order))
                       .ThenInclude(e => e.Node!.Tree)
                       .Include(e => e.RequiredNodes)
                       .ThenInclude(e => e.Node!.Tree)
                       .Include(e => e.Tree)
                : roles.Include(e => e.Nodes.OrderBy(rn => rn.Order))
                       .Include(e => e.RequiredNodes);
        public static IQueryable<Tree> IncludeSubordinate(this IQueryable<Tree> trees, bool includeNavigation = false) => includeNavigation
                ? trees.Include(e => e.RequiredNodes)
                       .ThenInclude(e => e.Node)
                : trees.Include(e => e.RequiredNodes);
        public static IQueryable<Node> IncludeSubordinate(this IQueryable<Node> nodes, bool includeNavigation = false) => includeNavigation
                ? nodes.Include(e => e.Tree)
                : nodes;
    }
}
