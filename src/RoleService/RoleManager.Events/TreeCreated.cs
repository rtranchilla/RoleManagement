namespace RoleManager.Events;

public sealed class TreeCreated
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}

//public record TreeCreated(Tree Tree) : INotification;

//public class TreeCreatedHandler : PubSubNotificationHandler<TreeCreated, Payloads.TreeCreated>
//{
//    public TreeCreatedHandler(IMapper mapper, DaprClient daprClient) : base(mapper, daprClient, PubSubSpec.TopicTrees) { }

//    public override Task<Payloads.TreeCreated> GeneratePayload(TreeCreated notification, IMapper mapper, CancellationToken cancellationToken) =>
//        Task.Run(() => mapper.Map<Tree, Payloads.TreeCreated>(notification.Tree), cancellationToken);
//}