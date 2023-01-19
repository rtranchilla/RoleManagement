using AutoMapper;
using RoleManagement.RoleManagementService;
using AutoMapper.EquivalencyExpression;

namespace RoleManagement.RoleManagementService.MapperConfig;

public sealed class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, Dto.Role>().EqualityComparison((s, d) => s.Id == d.Id)
                                   .ForMember(dest => dest.Name, cfg => cfg.MapFrom(src => string.Join("_", src.Nodes.Select(n => n.Node!.Name))))
                                   .ForMember(dest => dest.RequiredNodes, cfg => cfg.MapFrom(src => src.RequiredNodes.Select(e => e.NodeId)))
                                   .ReverseMap()
                                   .ForCtorParam("nodeIds", cfg => cfg.MapFrom((src, ctx) => ctx.Items["nodeIds"]))
                                   .EqualityComparison((s, d) => s.Id == d.Id);
    }
}