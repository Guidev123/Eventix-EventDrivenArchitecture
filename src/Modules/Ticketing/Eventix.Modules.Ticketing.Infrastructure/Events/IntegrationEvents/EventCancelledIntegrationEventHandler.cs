using Eventix.Modules.Events.IntegrationEvents.Events;
using Eventix.Modules.Ticketing.Application.Events.UseCases.Cancel;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;

namespace Eventix.Modules.Ticketing.Infrastructure.Events.IntegrationEvents
{
    internal sealed class EventCancelledIntegrationEventHandler(IMediatorHandler mediator) : IntegrationEventHandler<EventCancelledIntegrationEvent>
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