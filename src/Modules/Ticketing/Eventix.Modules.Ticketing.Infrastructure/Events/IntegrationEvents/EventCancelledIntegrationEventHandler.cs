using Eventix.Modules.Events.IntegrationEvents.Events;
using Eventix.Modules.Ticketing.Application.Events.UseCases.Cancel;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;
using MidR.Interfaces;

namespace Eventix.Modules.Ticketing.Infrastructure.Events.IntegrationEvents
{
    internal sealed class EventCancelledIntegrationEventHandler(IMediator mediator) : IntegrationEventHandler<EventCancelledIntegrationEvent>
    {
        public override async Task ExecuteAsync(EventCancelledIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var result = await mediator.DispatchAsync(new CancelEventCommand(
                integrationEvent.EventId), cancellationToken);

            if (result.IsFailure)
                throw new EventixException(nameof(CancelEventCommand), result.Error);
        }
    }
}