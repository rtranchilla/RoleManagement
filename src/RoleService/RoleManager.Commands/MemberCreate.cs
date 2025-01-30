using AutoMapper;
using RoleManager.DataPersistence;
using MediatR;
using RoleManager.Events;
using Microsoft.EntityFrameworkCore;
using Dapr.Client;
using Microsoft.Extensions.Configuration;
using RoleManager.Configuration;

namespace RoleManager.Commands;

public sealed record MemberCreate(Dto.Member Member) : AggregateRootCreate;
public sealed class MemberCreateHandler(RoleDbContext dbContext, IMapper mapper, DaprClient daprClient, PubSubConfiguration configuration) : AggregateRootCreateHandler<MemberCreate, Member, Dto.Member>(dbContext, mapper)
{
    protected override Dto.Member GetDto(MemberCreate request) => request.Member;
    protected override async Task PostSave(Member aggregateRoot, RoleDbContext dbContext, CancellationToken cancellationToken) =>
        await daprClient.PublishEventAsync(
            configuration.Name,
            configuration.Topic.Members.Created,
            new MemberCreated
            {
                Id = aggregateRoot.Id,
                NodeIds = MemberFunctions.GetNodeIds(dbContext, aggregateRoot.Id),
                UniqueName = aggregateRoot.UniqueName
            }, cancellationToken);
}