using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoleManager.DataPersistence;
using RoleManager.WebService.Configuration;

namespace RoleManager.WebService.Controllers;

#if DEBUG
[Route("[controller]")]
[ApiController]
public class TestDataController : ControllerBase
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHttpContextAccessor _contextAccessor;

    public TestDataController(IServiceScopeFactory serviceScopeFactory, IHttpContextAccessor contextAccessor)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _contextAccessor = contextAccessor;
    }

    [HttpPost]
    public IActionResult Post()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            RoleDbContext dbContext = scope.ServiceProvider.GetRequiredService<RoleDbContext>();
            Load(dbContext);
        }
        return Ok();
    }

    public static void Load(RoleDbContext dbContext)
    {
        if (dbContext.Members!.FirstOrDefault(e => e.UniqueName == "Member1") == null)
        {
            var trees = new List<Tree>
            {
                new Tree("Tree1"),
                new Tree("Tree2"),
                new Tree("Tree3")
            };
            dbContext.Trees!.AddRange(trees);

            var nodes = new List<Node>
            {
                new Node("Base1", trees[0].Id),
                new Node("Base2", trees[0].Id),
                new Node("Base3", trees[0].Id),

                new Node("Child1", trees[0].Id),//3
                new Node("Child2", trees[0].Id),
                new Node("Child3", trees[0].Id),

                new Node("SubChild1", trees[0].Id),//6
                new Node("SubChild2", trees[0].Id),

                new Node("SubSubChild1", trees[0].Id),//8

                new Node("Base1", trees[1].Id),//9
                new Node("Base2", trees[1].Id),

                new Node("Child1", trees[1].Id),//11
                new Node("Child2", trees[1].Id),

                new Node("SubChild1", trees[1].Id),//13

                new Node("Base1", trees[2].Id),//14
                new Node("Base2", trees[2].Id),
            };
            dbContext.Nodes!.AddRange(nodes);

            var roles = new List<Role>
            {
                new Role(Guid.NewGuid(), trees[0].Id, nodes[0].Id),//0
                new Role(Guid.NewGuid(), trees[0].Id, nodes[0].Id, nodes[3].Id),
                new Role(Guid.NewGuid(), trees[0].Id, nodes[0].Id, nodes[4].Id),
                new Role(Guid.NewGuid(), trees[0].Id, nodes[0].Id, nodes[4].Id, nodes[6].Id),
                new Role(Guid.NewGuid(), trees[0].Id, nodes[0].Id, nodes[4].Id, nodes[7].Id),
                new Role(Guid.NewGuid(), trees[0].Id, nodes[0].Id, nodes[5].Id, nodes[7].Id),
                new Role(Guid.NewGuid(), trees[0].Id, nodes[1].Id),//6
                new Role(Guid.NewGuid(), trees[0].Id, nodes[1].Id, nodes[4].Id),
                new Role(Guid.NewGuid(), trees[0].Id, nodes[1].Id, nodes[4].Id, nodes[6].Id),
                new Role(Guid.NewGuid(), trees[0].Id, nodes[1].Id, nodes[4].Id, nodes[7].Id),
                new Role(Guid.NewGuid(), trees[0].Id, nodes[1].Id, nodes[4].Id, nodes[7].Id, nodes[8].Id),
                new Role(Guid.NewGuid(), trees[0].Id, nodes[1].Id, nodes[5].Id),
                new Role(Guid.NewGuid(), trees[1].Id, nodes[9].Id),//12
                new Role(Guid.NewGuid(), trees[1].Id, nodes[9].Id, nodes[11].Id, nodes[13].Id),
                new Role(Guid.NewGuid(), trees[1].Id, nodes[9].Id, nodes[12].Id),
                new Role(Guid.NewGuid(), trees[1].Id, nodes[10].Id),
                new Role(Guid.NewGuid(), trees[1].Id, nodes[10].Id, nodes[11].Id),
                new Role(Guid.NewGuid(), trees[1].Id, nodes[10].Id, nodes[11].Id, nodes[13].Id),
                new Role(Guid.NewGuid(), trees[2].Id, nodes[14].Id),//18
                new Role(Guid.NewGuid(), trees[2].Id, nodes[15].Id),
            };
            dbContext.Roles!.AddRange(roles);

            var members = new List<Member>
            {
                new Member("Member1", "Member1"),
                new Member("Member2", "Member2"),
                new Member("Member3", "Member3"),
                new Member("Member4", "Member4"),
                new Member("Member5", "Member5"),
                new Member("Member6", "Member6"),
                new Member("Member7", "Member7"),
                new Member("Member8", "Member8"),
                new Member("Member9", "Member9"),
                new Member("Member10", "Member10")
            };
            members[0].Roles.Add(new MemberRole(members[0].Id, roles[0].TreeId, roles[0].Id));
            members[0].Roles.Add(new MemberRole(members[0].Id, roles[13].TreeId, roles[13].Id));
            members[1].Roles.Add(new MemberRole(members[1].Id, roles[10].TreeId, roles[10].Id));
            members[2].Roles.Add(new MemberRole(members[2].Id, roles[18].TreeId, roles[18].Id));

            dbContext.Members!.AddRange(members);

            dbContext.SaveChanges();
        }
    }
}
#endif
