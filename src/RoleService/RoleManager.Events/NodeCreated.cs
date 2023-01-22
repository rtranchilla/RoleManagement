namespace RoleManager.Events;

public record NodeCreated(Node Node) : INotification;

public class NodeCreatedHandler : PubSubNotificationHandler<NodeCreated, Payloads.NodeCreated>
{
    public NodeCreatedHandler(IMapper mapper, DaprClient daprClient) : base(mapper, daprClient, PubSubSpec.TopicNodes) { }

    public override Task<Payloads.NodeCreated> GeneratePayload(NodeCreated notification, IMapper mapper, CancellationToken cancellationToken) => 
        Task.Run(() => mapper.Map<Node, Payloads.NodeCreated>(notification.Node), cancellationToken);
}