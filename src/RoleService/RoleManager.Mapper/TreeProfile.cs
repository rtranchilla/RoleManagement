using AutoMapper;
using RoleManagement.RoleManagementService;
using AutoMapper.EquivalencyExpression;

namespace RoleManagement.RoleManagementService.MapperConfig;

public sealed class TreeProfile : Profile
{
    public TreeProfile()
    {
        CreateMap<Tree, Dto.Tree>().EqualityComparison((s, d) => s.Id == d.Id)
                                   .ForMember(dest => dest.RequiredNodes, cfg => cfg.MapFrom(src => src.RequiredNodes.Select(e => e.NodeId)))
                                   .ReverseMap()
                                   .EqualityComparison((s, d) => s.Id == d.Id);
    }
}