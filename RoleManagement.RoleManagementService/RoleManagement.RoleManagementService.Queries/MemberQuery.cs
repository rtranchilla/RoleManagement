﻿using AutoMapper;
using RoleManagement.RoleManagementService.DataPersistence;

namespace RoleManagement.RoleManagementService.Queries;

public sealed record MemberQuery : AggregateRootQuery<Dto.Member>
{
    public MemberQuery() { }
    public MemberQuery(Guid id) => Id = id;
    public MemberQuery(string uniqueName)
    {
        if (string.IsNullOrWhiteSpace(uniqueName))
            throw new ArgumentException($"'{nameof(uniqueName)}' cannot be null or whitespace.", nameof(uniqueName));        

        UniqueName = uniqueName;
    }

    public Guid? Id { get; }
    public string? UniqueName { get; }
}

public sealed class MemberQueryHandler : AggregateRootQueryHandler<MemberQuery, Member, Dto.Member>
{
    public MemberQueryHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

    protected override IQueryable<Member> QueryEntities(MemberQuery request, RoleDbContext dbContext)
    {
        if (request.Id != null)
            return dbContext.Members!.Where(e => e.Id == request.Id);
        if (request.UniqueName != null)
            return dbContext.Members!.Where(e => e.UniqueName == request.UniqueName);

        return dbContext.Members!;  
    }
}
