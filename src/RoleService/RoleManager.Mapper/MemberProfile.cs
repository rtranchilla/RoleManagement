using AutoMapper;
using RoleManagement.RoleManagementService;
using AutoMapper.EquivalencyExpression;

namespace RoleManagement.RoleManagementService.MapperConfig;

public sealed class MemberProfile : Profile
{
    public MemberProfile()
    {
        CreateMap<Member, Dto.Member>().EqualityComparison((s, d) => s.Id == d.Id)
                                       .ReverseMap()
                                       .EqualityComparison((s, d) => s.Id == d.Id);
    }
}