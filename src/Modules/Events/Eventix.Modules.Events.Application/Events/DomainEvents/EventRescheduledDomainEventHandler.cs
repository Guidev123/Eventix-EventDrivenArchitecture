using Eventix.Modules.Events.Domain.Events.DomainEvents;
using Eventix.Modules.Events.IntegrationEvents.Events;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.DomainEvents
{
    internal sealed class EventRescheduledDomainEventHandler(IBus eventBus) : DomainEventHandler<EventRescheduledDomainEvent>
    {
        public override async Task ExecuteAsync(EventRescheduledDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await eventBus.PublishAsync(new EventRescheduledIntegrationEvent(
                domainEvent.CorrelationId,
                domainEvent.OccurredOnUtc,
                domainEvent.EventId,
                domainEvent.StartsAtUtc,
                domainEvent.EndsAtUtc), cancellationToken);
        }
    }
}