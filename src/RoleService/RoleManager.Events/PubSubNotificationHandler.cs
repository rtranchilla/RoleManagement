global using AutoMapper;
global using Dapr.Client;
global using MediatR;

namespace RoleManager.Events
{
    public abstract class PubSubNotificationHandler<TNotification, TPayload> : INotificationHandler<TNotification>
        where TNotification : INotification
    {
        private readonly IMapper mapper;
        private readonly DaprClient daprClient;
        private readonly string topic;

        public PubSubNotificationHandler(IMapper mapper, DaprClient daprClient, string topic)
        {
            this.mapper = mapper;
            this.daprClient = daprClient;
            this.topic = topic;
        }

        public abstract Task<TPayload> GeneratePayload(TNotification notification, IMapper mapper, CancellationToken cancellationToken);

        public async Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            var payload = await GeneratePayload(notification, mapper, cancellationToken);
            await daprClient.PublishEventAsync(PubSubSpec.Name, topic, payload, cancellationToken);
        }
    }
}
