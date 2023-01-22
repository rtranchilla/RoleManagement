using AutoMapper;
using RoleManager.Events.Payloads;

namespace RoleManager.Mapper;

public sealed class TreePayloadProfile : Profile
{
	public TreePayloadProfile()
	{
		CreateMap<Tree, TreeCreated>();
		CreateMap<Tree, TreeDeleted>();
    }
}
