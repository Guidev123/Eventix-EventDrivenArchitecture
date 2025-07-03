namespace Eventix.Shared.Application.EventBus
{
    public abstract class IntegrationEventHandler<TIntegrationEvent> : IIntegrationEventHandler<TIntegrationEvent>
        where TIntegrationEvent : IIntegrationEvent
    {
        public abstract Task ExecuteAsync(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);

        public Task ExecuteAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default) =>
            ExecuteAsync((TIntegrationEvent)integrationEvent, cancellationToken);
    }
}