namespace RoleManager.Events;

public sealed class MemberDeleted
{
    public Guid Id { get; set; }
}

//public record MemberDeleted(Guid Id) : INotification;

//public class MemberDeletedHandler : PubSubNotificationHandler<MemberDeleted, Payloads.MemberDeleted>
//{
//    public MemberDeletedHandler(IMapper mapper, DaprClient daprClient) : base(mapper, daprClient, PubSubSpec.TopicMembers) { }

//    public override Task<Payloads.MemberDeleted> GeneratePayload(MemberDeleted notification, IMapper mapper, CancellationToken cancellationToken) =>
//        Task.Run(() => new Payloads.MemberDeleted { Id = notification.Id }, cancellationToken);
//}