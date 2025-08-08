using Eventix.Modules.Events.Domain.TicketTypes.DomainEvents;
using Eventix.Modules.Events.IntegrationEvents.TicketTypes;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.TicketTypes.DomainEvents
{
    internal sealed class TicketTypePriceChangedDomainEventHandler(IBus eventBus) : DomainEventHandler<TicketTypePriceChangedDomainEvent>
    {
        public override async Task ExecuteAsync(TicketTypePriceChangedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await eventBus.PublishAsync(new TicketTypePriceChangedIntegrationEvent(
                domainEvent.CorrelationId,
                domainEvent.OccurredOnUtc,
                domainEvent.TicketTypeId,
                domainEvent.Price
                ), cancellationToken);
        }
    }
}