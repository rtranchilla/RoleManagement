using AutoMapper;
using RoleManager;
using AutoMapper.EquivalencyExpression;

namespace RoleManager.MapperConfig;

public sealed class TreeProfile : Profile
{
    public TreeProfile()
    {
        CreateMap<Tree, Dto.Tree>().ForMember(dest => dest.RequiredNodes, cfg => cfg.MapFrom(src => src.RequiredNodes.Select(e => e.NodeId)))
                                   .EqualityComparison((s, d) => s.Id == d.Id)
                                   .ReverseMap()
                                   .ForMember(dest => dest.RequiredNodes, cfg => cfg.Ignore())
                                   .EqualityComparison((s, d) => s.Id == d.Id);
        CreateMap<Dto.TreeUpdate, Tree>().ForMember(dest => dest.RequiredNodes, cfg => cfg.Ignore())
                                         .EqualityComparison((s, d) => s.Id == d.Id);
    }
}