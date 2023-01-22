using AutoMapper;
using RoleManager.Events.Payloads;

namespace RoleManager.Mapper;

public sealed class MemberPayloadProfile : Profile
{
	public MemberPayloadProfile()
	{
		CreateMap<Member, MemberCreated>().ForMember(dest => dest.NodeIds, opt => opt.MapFrom((src, dest, sm, ctx) => ctx.Items["nodeIds"]));
		CreateMap<Member, MemberUpdated>().ForMember(dest => dest.NodeIds, opt => opt.MapFrom((src, dest, sm, ctx) => ctx.Items["nodeIds"]));
		CreateMap<Member, MemberDeleted>();
    }
}
