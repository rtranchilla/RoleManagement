using AutoMapper;

namespace RoleManager.PowerShell.Mapper;

public class TreeProfile : Profile
{
	public TreeProfile()
	{
		CreateMap<Tree, Dto.Tree>().ReverseMap();
		CreateMap<Tree, Dto.TreeUpdate>();
	}
}