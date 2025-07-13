using Eventix.Modules.Attendance.Application.Events.UseCases.Create;
using Eventix.Modules.Events.IntegrationEvents.Events;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;

namespace Eventix.Modules.Attendance.Infrastructure.Events.IntegrationEvents
{
    internal sealed class EventPublishedIntegrationEventHandler(IMediatorHandler mediator) : IntegrationEventHandler<EventPublishedIntegrationEvent>
    {
        public override async Task ExecuteAsync(EventPublishedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var result = await mediator.DispatchAsync(new CreateEventCommand(
                integrationEvent.EventId,
                integrationEvent.Title,
                integrationEvent.Description,
                MapToLocationRequest(integrationEvent.Location),
                integrationEvent.StartsAtUtc,
                integrationEvent.EndsAtUtc
                ), cancellationToken);

            if (result.IsFailure)
                throw new EventixException(nameof(CreateEventCommand), result.Error);
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