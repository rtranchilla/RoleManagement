namespace RoleConfiguration.Mapper;

public sealed class MemberProfile : Profile
{
	public MemberProfile()
	{
		CreateMap<MemberContent, RoleManager.Dto.Member>()
			.EqualityComparison((s, d) => s.UniqueName == d.UniqueName)
			.ReverseMap()
			.EqualityComparison((s, d) => s.UniqueName == d.UniqueName);
		CreateMap<Member, RoleManager.Dto.Member>()
			.EqualityComparison((s, d) => s.Id == d.Id)
			.ReverseMap()
            .EqualityComparison((s, d) => s.Id == d.Id);
        CreateMap<MemberContent, Member>();

        CreateMap<RoleManager.Dto.Member, RoleManager.Dto.MemberUpdate>();
		CreateMap<MemberContent, RoleManager.Dto.MemberUpdate>();
	}
}