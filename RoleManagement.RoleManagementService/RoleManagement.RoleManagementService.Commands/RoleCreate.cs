using AutoMapper;
using RoleManagement.RoleManagementService.DataPersistence;
namespace RoleManagement.RoleManagementService.Commands;

public sealed record RoleCreate(Dto.Role Role) : AggregateRootCreate;
public sealed class RoleCreateHandler : AggregateRootCreateHandler<RoleCreate, Role, Dto.Role>
{
    public RoleCreateHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

    private List<Node>? nodes;

    protected override async Task PreCreate(RoleCreate request, RoleDbContext dbContext)
    {
        nodes = new List<Node>();
        foreach (var nodeName in request.Role.Name.Split('_')) 
        {
            var node = dbContext.Nodes!.FirstOrDefault(e => e.Name == nodeName);
            if (node == null)
            {
                node = new Node(nodeName, request.Role.TreeId);
                node.BaseNode = nodes.Count == 0;
                await dbContext.Nodes!.AddAsync(node);
            }
            nodes.Add(node);
        }
        
    }

    protected override Role Map(Dto.Role dto, IMapper mapper) => mapper.Map<Dto.Role, Role>(dto, opt => opt.Items["nodeIds"] = nodes!.Select(e => e.Id).ToArray());
    protected override Dto.Role GetDto(RoleCreate request) => request.Role;
}