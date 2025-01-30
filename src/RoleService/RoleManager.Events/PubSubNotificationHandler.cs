//global using AutoMapper;
//global using Dapr.Client;
//global using MediatR;

//namespace RoleManager.Events
//{
//    public abstract class PubSubNotificationHandler<TNotification, TPayload>(IMapper mapper, DaprClient daprClient, string topic) : INotificationHandler<TNotification>
//        where TNotification : INotification
//    {
//        public abstract Task<TPayload> GeneratePayload(TNotification notification, IMapper mapper, CancellationToken cancellationToken);

//        public async Task Handle(TNotification notification, CancellationToken cancellationToken)
//        {
//            var payload = await GeneratePayload(notification, mapper, cancellationToken);
//            await daprClient.PublishEventAsync(PubSubSpec.Name, topic, payload, cancellationToken);
//        }
//    }
//}
