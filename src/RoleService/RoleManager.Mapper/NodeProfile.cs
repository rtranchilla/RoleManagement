using AutoMapper;
using RoleManager;
using AutoMapper.EquivalencyExpression;

namespace RoleManager.MapperConfig;

public sealed class NodeProfile : Profile
{
    public NodeProfile()
    {
        CreateMap<Node, Dto.Node>().EqualityComparison((s, d) => s.Id == d.Id)
                                   .ReverseMap()
                                   .EqualityComparison((s, d) => s.Id == d.Id);
    }
}