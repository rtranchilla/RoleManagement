using AutoMapper;

namespace RoleManager.PowerShell.Mapper;

public class RoleProfile : Profile
{
	public RoleProfile()
	{
		CreateMap<Role, Dto.Role>().ReverseMap();
		CreateMap<Role, Dto.RoleUpdate>();
	}
}