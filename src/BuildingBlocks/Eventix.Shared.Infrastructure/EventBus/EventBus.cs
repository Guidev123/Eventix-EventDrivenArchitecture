using Eventix.Shared.Application.EventBus;
using MassTransit;

namespace Eventix.Shared.Infrastructure.EventBus
{
    internal sealed class EventBus(IBus bus) : IEventBus
    {
        public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken)
            where T : class, IIntegrationEvent
        {
            await bus.Publish(integrationEvent, cancellationToken);
        }
    }
}