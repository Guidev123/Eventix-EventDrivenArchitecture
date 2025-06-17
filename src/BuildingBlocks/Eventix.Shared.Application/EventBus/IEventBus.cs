namespace Eventix.Shared.Application.EventBus
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
            where T : class, IIntegrationEvent;
    }
}