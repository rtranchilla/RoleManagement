namespace RoleConfiguration.Mapper;

public sealed class TreeProfile : Profile
{
	public TreeProfile()
	{
        CreateMap<TreeContent, RoleManager.Dto.Tree>()
            .EqualityComparison((s, d) => s.Name == d.Name)
            .ReverseMap()
            .EqualityComparison((s, d) => s.Name == d.Name);
        CreateMap<Tree, RoleManager.Dto.Tree>()
            .EqualityComparison((s, d) => s.Id == d.Id)
            .ReverseMap()
            .EqualityComparison((s, d) => s.Id == d.Id);
    }
}
