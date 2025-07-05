using Eventix.Modules.Attendance.Application.Tickets.UseCases.Create;
using Eventix.Modules.Ticketing.IntegrationEvents.Tickets;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;
using MidR.Interfaces;

namespace Eventix.Modules.Attendance.Infrastructure.Tickets.IntegrationEvents
{
    internal sealed class TicketCreatedIntegrationEventHandler(IMediator mediator) : IntegrationEventHandler<TicketCreatedIntegrationEvent>
    {
        public override async Task ExecuteAsync(TicketCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var result = await mediator.DispatchAsync(new CreateTicketCommand(
                integrationEvent.TicketId,
                integrationEvent.AttendeeId,
                integrationEvent.EventId,
                integrationEvent.Code
                ), cancellationToken);

            if (result.IsFailure)
                throw new EventixException(nameof(CreateTicketCommand), result.Error);
        }
    }
}