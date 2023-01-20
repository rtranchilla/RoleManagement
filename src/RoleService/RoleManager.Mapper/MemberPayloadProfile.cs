using AutoMapper;
using RoleManager.Events.Payloads;

namespace RoleManager.Mapper;

public sealed class MemberPayloadProfile : Profile
{
	public MemberPayloadProfile()
	{
		CreateMap<Member, MemberCreated>();
	}
}
