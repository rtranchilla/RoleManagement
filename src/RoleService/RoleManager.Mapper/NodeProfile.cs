using AutoMapper;
using RoleManagement.RoleManagementService;
using AutoMapper.EquivalencyExpression;

namespace RoleManagement.RoleManagementService.MapperConfig;

public sealed class NodeProfile : Profile
{
    public NodeProfile()
    {
        CreateMap<Node, Dto.Node>().EqualityComparison((s, d) => s.Id == d.Id)
                                   .ReverseMap()
                                   .EqualityComparison((s, d) => s.Id == d.Id);
    }
}