using AutoMapper;
using AutoMapper.EquivalencyExpression;

namespace RoleManager.MapperConfig;

public sealed class MemberProfile : Profile
{
    public MemberProfile()
    {
        CreateMap<Member, Dto.Member>().EqualityComparison((s, d) => s.Id == d.Id)
                                       .ReverseMap()
                                       .EqualityComparison((s, d) => s.Id == d.Id);

        CreateMap<Dto.MemberUpdate, Member>().EqualityComparison((s, d) => s.Id == d.Id);
    }
}