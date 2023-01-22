namespace RoleManager.Events;

public record MemberCreated(Member Member, Guid[] NodeIds) : INotification;

public class MemberCreatedHandler : PubSubNotificationHandler<MemberCreated, Payloads.MemberCreated>
{
    public MemberCreatedHandler(IMapper mapper, DaprClient daprClient) : base(mapper, daprClient, PubSubSpec.TopicMembers) { }

    public override Task<Payloads.MemberCreated> GeneratePayload(MemberCreated notification, IMapper mapper, CancellationToken cancellationToken) => 
        Task.Run(() => mapper.Map<Member, Payloads.MemberCreated>(notification.Member, opt => opt.Items["nodeIds"] = notification.NodeIds), cancellationToken);
}