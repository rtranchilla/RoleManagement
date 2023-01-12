using AutoMapper;
using RoleManagement.RoleManagementService;
using AutoMapper.EquivalencyExpression;

namespace RoleManagement.RoleManagementService.MapperConfig;

public sealed class TreeProfile : Profile
{
    public TreeProfile()
    {
        CreateMap<Tree, Dto.Tree>().EqualityComparison((s, d) => s.Id == d.Id)
                                   .ReverseMap()
                                   .EqualityComparison((s, d) => s.Id == d.Id);
    }
}