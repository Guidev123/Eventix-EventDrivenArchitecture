using Eventix.Modules.Events.Application.TicketTypes.UseCases.GetById;
using Eventix.Modules.Events.PublicApi;
using Eventix.Modules.Events.PublicApi.TicketTypes.Responses;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Infrastructure.PublicApi
{
    internal sealed class EventsApi(IMediator mediator) : IEventsApi
    {
        public async Task<TicketTypeResponse?> GetTicketTypeAsync(Guid ticketTypeId, CancellationToken cancellationToken = default)
        {
            var result = await mediator.DispatchAsync(new GetTicketTypeByIdQuery(ticketTypeId), cancellationToken);
            if (result.IsFailure) return null;

            return new(
                result.Value.Id,
                result.Value.EventId,
                result.Value.Name,
                result.Value.Amount,
                result.Value.Currency,
                result.Value.Quantity
                );
        }
    }
}