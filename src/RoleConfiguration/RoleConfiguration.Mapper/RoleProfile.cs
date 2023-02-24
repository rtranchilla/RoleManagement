namespace RoleConfiguration.Mapper;

public sealed class RoleProfile : Profile
{
	public RoleProfile()
    {
        CreateMap<RoleContent, RoleManager.Dto.Role>()
            .EqualityComparison((s, d) => s.Name == d.Name)
            .ReverseMap()
            .EqualityComparison((s, d) => s.Name == d.Name);
        CreateMap<Member, RoleManager.Dto.Role>()
            .EqualityComparison((s, d) => s.Id == d.Id)
            .ReverseMap()
            .EqualityComparison((s, d) => s.Id == d.Id);
    }
}
