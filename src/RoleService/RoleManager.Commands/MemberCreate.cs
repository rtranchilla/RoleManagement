using AutoMapper;
using RoleManager.DataPersistence;
using MediatR;
using RoleManager.Events;
using Microsoft.EntityFrameworkCore;

namespace RoleManager.Commands;

public sealed record MemberCreate(Dto.Member Member) : AggregateRootCreate;
public sealed class MemberCreateHandler : AggregateRootCreateHandler<MemberCreate, Member, Dto.Member>
{
    private readonly IPublisher publisher;

    public MemberCreateHandler(RoleDbContext dbContext, IMapper mapper, IPublisher publisher) : base(dbContext, mapper) => this.publisher = publisher;

    protected override Dto.Member GetDto(MemberCreate request) => request.Member;
    protected override Task PostSave(Member aggregateRoot, RoleDbContext dbContext, CancellationToken cancellationToken) => 
        publisher.Publish(new MemberCreated(aggregateRoot, MemberFunctions.GetNodeIds(dbContext, aggregateRoot.Id)), cancellationToken);
}