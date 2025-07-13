using Eventix.Modules.Events.IntegrationEvents.Events;
using Eventix.Modules.Ticketing.Application.Events.UseCases.Create;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;

namespace Eventix.Modules.Ticketing.Infrastructure.Events.IntegrationEvents
{
    internal sealed class EventPublishedIntegrationEventHandler(IMediatorHandler mediator) : IntegrationEventHandler<EventPublishedIntegrationEvent>
    {
        public override async Task ExecuteAsync(EventPublishedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var command = new CreateEventCommand(
                integrationEvent.EventId,
                integrationEvent.Title,
                integrationEvent.Description,
                MapToLocationRequest(integrationEvent.Location),
                integrationEvent.StartsAtUtc,
                integrationEvent.EndsAtUtc,
                MapToTicketTypeRequest(integrationEvent.TicketTypes)
                );

            var result = await mediator.DispatchAsync(command, cancellationToken);

            if (result.IsFailure)
                throw new EventixException(nameof(CreateEventCommand), result.Error);
        }

        private static List<CreateEventCommand.TicketTypeRequest> MapToTicketTypeRequest(IReadOnlyCollection<EventPublishedIntegrationEvent.TicketTypeRequest> ticketTypes)
        {
            return ticketTypes.Select(tt => new CreateEventCommand.TicketTypeRequest(
                    tt.TicketTypeId,
                    tt.EventId,
                    tt.Name,
                    tt.Price,
                    tt.Currency,
                    tt.Quantity
                )).ToList();
        }

        private static CreateEventCommand.LocationRequest? MapToLocationRequest(EventPublishedIntegrationEvent.LocationRequest? location)
        {
            return location is null ? null : new CreateEventCommand.LocationRequest(
                location.Street,
                location.Number,
                location.AdditionalInfo,
                location.Neighborhood,
                location.ZipCode,
                location.City,
                location.State
            );
        }
    }
}