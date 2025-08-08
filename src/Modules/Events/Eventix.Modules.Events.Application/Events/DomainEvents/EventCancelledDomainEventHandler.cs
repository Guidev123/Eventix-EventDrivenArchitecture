using Eventix.Modules.Events.Domain.Events.DomainEvents;
using Eventix.Modules.Events.IntegrationEvents.Events;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.DomainEvents
{
    internal sealed class EventCancelledDomainEventHandler(IBus eventBus) : DomainEventHandler<EventCancelledDomainEvent>
    {
        public override async Task ExecuteAsync(EventCancelledDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await eventBus.PublishAsync(new EventCancelledIntegrationEvent(
                domainEvent.CorrelationId,
                domainEvent.OccurredOnUtc,
                domainEvent.EventId), cancellationToken);
        }
    }
}