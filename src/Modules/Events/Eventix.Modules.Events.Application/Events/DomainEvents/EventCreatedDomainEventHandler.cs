using Eventix.Modules.Events.Application.Events.UseCases.GetById;
using Eventix.Modules.Events.Application.TicketTypes.Dtos;
using Eventix.Modules.Events.Application.TicketTypes.UseCases.GetByEventId;
using Eventix.Modules.Events.Domain.Events.DomainEvents;
using Eventix.Modules.Events.IntegrationEvents.Events;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;
using Eventix.Shared.Application.Messaging;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Application.Events.DomainEvents
{
    internal sealed class EventCreatedDomainEventHandler(IEventBus eventBus, IMediator mediator) : DomainEventHandler<EventCreatedDomainEvent>
    {
        public override async Task ExecuteAsync(EventCreatedDomainEvent notification, CancellationToken cancellationToken = default)
        {
            var eventResult = await mediator.DispatchAsync(new GetEventByIdQuery(notification.EventId), cancellationToken);
            var ticketTypesResult = await mediator.DispatchAsync(new GetTicketTypeByEventIdQuery(notification.EventId), cancellationToken);

            if (eventResult.IsFailure)
                throw new EventixException(nameof(GetEventByIdQuery), eventResult.Error);

            if (ticketTypesResult.IsFailure)
                throw new EventixException(nameof(GetTicketTypeByEventIdQuery), ticketTypesResult.Error);

            var @event = eventResult.Value;
            var ticketTypes = ticketTypesResult.Value.TicketTypes;

            await eventBus.PublishAsync(new EventCreatedIntegrationEvent(
                notification.Id,
                notification.OccurredOnUtc,
                @event.Id,
                @event.Title,
                @event.Description,
                @event.CategoryId,
                @event.StartsAtUtc,
                @event.EndsAtUtc,
                MapToLocationRequest(@event),
                MapToTicketTypeRequest(ticketTypes)), cancellationToken);
        }

        private static List<EventCreatedIntegrationEvent.TicketTypeRequest> MapToTicketTypeRequest(IReadOnlyCollection<TicketTypeDto> response)
        {
            return response.Select(tt => new EventCreatedIntegrationEvent.TicketTypeRequest(
                    tt.Id,
                    tt.EventId,
                    tt.Name,
                    tt.Amount,
                    tt.Currency,
                    tt.Quantity
                )).ToList();
        }

        private static EventCreatedIntegrationEvent.LocationRequest? MapToLocationRequest(GetEventResponse eventResponse)
        {
            return LocationIsNull(eventResponse) ? null : new EventCreatedIntegrationEvent.LocationRequest(
                eventResponse.Street!,
                eventResponse.Number!,
                eventResponse.AdditionalInfo!,
                eventResponse.Neighborhood!,
                eventResponse.ZipCode!,
                eventResponse.City!,
                eventResponse.State!
            );
        }

        private static bool LocationIsNull(GetEventResponse eventResponse)
            => string.IsNullOrWhiteSpace(eventResponse.Street) ||
                   string.IsNullOrWhiteSpace(eventResponse.City) ||
                   string.IsNullOrEmpty(eventResponse.Number) ||
                   string.IsNullOrWhiteSpace(eventResponse.AdditionalInfo) ||
                   string.IsNullOrWhiteSpace(eventResponse.Neighborhood) ||
                   string.IsNullOrWhiteSpace(eventResponse.State) ||
                   string.IsNullOrWhiteSpace(eventResponse.ZipCode);
    }
}