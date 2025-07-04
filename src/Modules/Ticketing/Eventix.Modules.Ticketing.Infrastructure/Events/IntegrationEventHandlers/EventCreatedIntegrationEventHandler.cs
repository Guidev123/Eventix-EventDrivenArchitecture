using Eventix.Modules.Events.IntegrationEvents.Events;
using Eventix.Modules.Ticketing.Application.Events.UseCases.Create;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;
using MidR.Interfaces;

namespace Eventix.Modules.Ticketing.Infrastructure.Events.IntegrationEventHandlers
{
    internal sealed class EventCreatedIntegrationEventHandler(IMediator mediator) : IntegrationEventHandler<EventCreatedIntegrationEvent>
    {
        public override async Task ExecuteAsync(EventCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
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

        private static List<CreateEventCommand.TicketTypeRequest> MapToTicketTypeRequest(IReadOnlyCollection<EventCreatedIntegrationEvent.TicketTypeRequest> ticketTypes)
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

        private static CreateEventCommand.LocationRequest? MapToLocationRequest(EventCreatedIntegrationEvent.LocationRequest? location)
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