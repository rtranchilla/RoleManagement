namespace RoleConfiguration.Mapper;

public sealed class RoleProfile : Profile
{
	public RoleProfile()
    {
        CreateMap<RoleContent, RoleManager.Dto.Role>()
            .EqualityComparison((s, d) => s.Name == d.Name)
            .ForMember(dest => dest.RequiredNodes, cfg => cfg.Ignore())
            .ReverseMap()
            .EqualityComparison((s, d) => s.Name == d.Name)
            .ForMember(dest => dest.RequiredNodes, cfg => cfg.Ignore());
        CreateMap<Role, RoleManager.Dto.Role>()
            .EqualityComparison((s, d) => s.Id == d.Id)
            .ReverseMap()
            .EqualityComparison((s, d) => s.Id == d.Id);
        CreateMap<RoleContent, Role>();

        CreateMap<RoleManager.Dto.Role, RoleManager.Dto.RoleUpdate>();
        CreateMap<RoleContent, RoleManager.Dto.RoleUpdate>()
            .ForMember(dest => dest.RequiredNodes, cfg => cfg.Ignore());
    }
}
