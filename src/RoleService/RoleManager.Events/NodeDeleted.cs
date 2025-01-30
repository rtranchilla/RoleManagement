namespace RoleManager.Events;

public sealed class NodeDeleted
{
    public Guid Id { get; set; }
}


//public record NodeDeleted(Guid Id) : INotification;

//public class NodeDeletedHandler : PubSubNotificationHandler<NodeDeleted, Payloads.NodeDeleted>
//{
//    public NodeDeletedHandler(IMapper mapper, DaprClient daprClient) : base(mapper, daprClient, PubSubSpec.TopicNodes) { }

//    public override Task<Payloads.NodeDeleted> GeneratePayload(NodeDeleted notification, IMapper mapper, CancellationToken cancellationToken) => 
//        Task.Run(() => new Payloads.NodeDeleted { Id = notification.Id }, cancellationToken);
//}