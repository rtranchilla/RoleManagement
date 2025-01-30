namespace RoleManager.Events;

public sealed class TreeDeleted
{
    public Guid Id { get; set; }
}

//public record TreeDeleted(Guid Id) : INotification;

//public class TreeDeletedHandler : PubSubNotificationHandler<TreeDeleted, Payloads.TreeDeleted>
//{
//    public TreeDeletedHandler(IMapper mapper, DaprClient daprClient) : base(mapper, daprClient, PubSubSpec.TopicTrees) { }

//    public override Task<Payloads.TreeDeleted> GeneratePayload(TreeDeleted notification, IMapper mapper, CancellationToken cancellationToken) =>
//        Task.Run(() => new Payloads.TreeDeleted { Id = notification.Id }, cancellationToken);
//}