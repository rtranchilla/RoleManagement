using AutoMapper;

namespace RoleManager.PowerShell.Mapper;

public class MemberProfile : Profile
{
	public MemberProfile()
	{
		CreateMap<Member, Dto.Member>().ReverseMap();
	}
}