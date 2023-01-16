using AutoMapper;
using RoleManagement.RoleManagementService.DataPersistence;
namespace RoleManagement.RoleManagementService.Commands;

public sealed record TreeCreate(Dto.Tree Tree) : AggregateRootCreate;
public sealed class TreeCreateHandler : AggregateRootCreateHandler<TreeCreate, Tree, Dto.Tree>
{
    public TreeCreateHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

    protected override Dto.Tree GetDto(TreeCreate request) => request.Tree;
}