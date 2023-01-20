using AutoMapper;
using Dapr.Client;
using MediatR;

namespace RoleManager.Events;

public record MemberCreated(Member Member) : INotification { }

public class MemberCreatedHandler : INotificationHandler<MemberCreated>
{
    private readonly IMapper mapper;
    private readonly DaprClient daprClient;

    public MemberCreatedHandler(IMapper mapper, DaprClient daprClient)
    {
        this.mapper = mapper;
        this.daprClient = daprClient;
    }

    public async Task Handle(MemberCreated notification, CancellationToken cancellationToken)
    {
        var payload = await Task.Run(() =>  mapper.Map<Member,Payloads.MemberCreated>(notification.Member), cancellationToken);
        await daprClient.PublishEventAsync(PubSubSpec.Name, PubSubSpec.TopicMembers, payload, cancellationToken);
    }
}

