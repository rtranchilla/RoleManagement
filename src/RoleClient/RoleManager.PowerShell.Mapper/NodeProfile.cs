using AutoMapper;

namespace RoleManager.PowerShell.Mapper;

public class NodeProfile : Profile
{
	public NodeProfile()
	{
		CreateMap<Node, Dto.Node>().ReverseMap();
	}
}