﻿using AutoMapper;
using RoleManager.Events;

namespace RoleManager.Mapper;

public sealed class NodePayloadProfile : Profile
{
	public NodePayloadProfile()
	{
		CreateMap<Node, NodeCreated>();
		CreateMap<Node, NodeDeleted>();
    }
}
