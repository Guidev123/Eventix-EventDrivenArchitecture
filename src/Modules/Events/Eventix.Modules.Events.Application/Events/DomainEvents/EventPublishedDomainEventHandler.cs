﻿using Eventix.Modules.Events.Application.Events.UseCases.GetById;
using Eventix.Modules.Events.Application.TicketTypes.DTOs;
using Eventix.Modules.Events.Application.TicketTypes.UseCases.GetByEventId;
using Eventix.Modules.Events.Domain.Events.DomainEvents;
using Eventix.Modules.Events.IntegrationEvents.Events;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.DomainEvents
{
    internal sealed class EventPublishedDomainEventHandler(IBus eventBus, IMediatorHandler mediator) : DomainEventHandler<EventPublishedDomainEvent>
    {
        public override async Task ExecuteAsync(EventPublishedDomainEvent notification, CancellationToken cancellationToken = default)
        {
            var eventResult = await mediator.DispatchAsync(new GetEventByIdQuery(notification.EventId), cancellationToken);
            var ticketTypesResult = await mediator.DispatchAsync(new GetTicketTypeByEventIdQuery(notification.EventId), cancellationToken);

            if (eventResult.IsFailure)
                throw new EventixException(nameof(GetEventByIdQuery), eventResult.Error);

            if (ticketTypesResult.IsFailure)
                throw new EventixException(nameof(GetTicketTypeByEventIdQuery), ticketTypesResult.Error);

            var @event = eventResult.Value;
            var ticketTypes = ticketTypesResult.Value.TicketTypes;

            await eventBus.PublishAsync(new EventPublishedIntegrationEvent(
                notification.CorrelationId,
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

        private static List<EventPublishedIntegrationEvent.TicketTypeRequest> MapToTicketTypeRequest(IReadOnlyCollection<TicketTypeResponse> response)
        {
            return response.Select(tt => new EventPublishedIntegrationEvent.TicketTypeRequest(
                    tt.Id,
                    tt.EventId,
                    tt.Name,
                    tt.Amount,
                    tt.Currency,
                    tt.Quantity
                )).ToList();
        }

        private static EventPublishedIntegrationEvent.LocationRequest? MapToLocationRequest(GetEventResponse eventResponse)
        {
            return LocationIsNull(eventResponse) ? null : new EventPublishedIntegrationEvent.LocationRequest(
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