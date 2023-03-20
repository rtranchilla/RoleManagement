namespace RoleConfiguration.Mapper;

public sealed class TreeProfile : Profile
{
	public TreeProfile()
	{
        CreateMap<TreeContent, RoleManager.Dto.Tree>()
            .EqualityComparison((s, d) => s.Name == d.Name)
            .ForMember(dest => dest.RequiredNodes, cfg => cfg.Ignore())
            .ReverseMap()
            .EqualityComparison((s, d) => s.Name == d.Name)
            .ForMember(dest => dest.RequiredNodes, cfg => cfg.Ignore());
        CreateMap<Tree, RoleManager.Dto.Tree>()
            .EqualityComparison((s, d) => s.Id == d.Id)
            .ReverseMap()
            .EqualityComparison((s, d) => s.Id == d.Id);
        CreateMap<TreeContent, Tree>();

        CreateMap<RoleManager.Dto.Tree, RoleManager.Dto.TreeUpdate>();
        CreateMap<Tree, RoleManager.Dto.TreeUpdate>();
        CreateMap<TreeContent, RoleManager.Dto.TreeUpdate>()
            .ForMember(dest => dest.RequiredNodes, cfg => cfg.Ignore());
    }
}
