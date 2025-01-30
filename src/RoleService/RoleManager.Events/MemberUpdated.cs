namespace RoleManager.Events;

public sealed class MemberUpdated
{
    public Guid Id { get; set; }
    public Guid[]? NodeIds { get; set; }
}

//public record MemberUpdated(Member Member, Guid[] NodeIds) : INotification;

//public class MemberUpdatedHandler : PubSubNotificationHandler<MemberUpdated, Payloads.MemberUpdated>
//{
//    public MemberUpdatedHandler(IMapper mapper, DaprClient daprClient) : base(mapper, daprClient, PubSubSpec.TopicMembers) { }

//    public override Task<Payloads.MemberUpdated> GeneratePayload(MemberUpdated notification, IMapper mapper, CancellationToken cancellationToken) =>
//        Task.Run(() => mapper.Map<Member, Payloads.MemberUpdated>(notification.Member, opt => opt.Items["nodeIds"] = notification.NodeIds), cancellationToken);
//}