using Eventix.Modules.Ticketing.Application.Tickets.UseCases.GetById;
using Eventix.Modules.Ticketing.Domain.Tickets.DomainEvents;
using Eventix.Modules.Ticketing.IntegrationEvents.Tickets;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;
using Eventix.Shared.Application.Messaging;
using MidR.Interfaces;

namespace Eventix.Modules.Ticketing.Application.Tickets.DomainEvents
{
    internal sealed class TicketCreatedDomainEventHandler(IEventBus eventBus, IMediator mediator) : DomainEventHandler<TicketCreatedDomainEvent>
    {
        public override async Task ExecuteAsync(TicketCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var ticketResult = await mediator.DispatchAsync(new GetTicketByIdQuery(domainEvent.TicketId), cancellationToken);

            if (ticketResult.IsFailure)
                throw new EventixException(nameof(GetTicketByIdQuery), ticketResult.Error);

            var ticket = ticketResult.Value;

            await eventBus.PublishAsync(new TicketCreatedIntegrationEvent(
                    domainEvent.Id,
                    domainEvent.OccurredOnUtc,
                    domainEvent.TicketId,
                    ticket.CustomerId,
                    ticket.EventId,
                    ticket.Code
                ),
                cancellationToken
            );
        }
    }
}