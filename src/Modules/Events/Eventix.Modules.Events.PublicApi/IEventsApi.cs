using Eventix.Modules.Events.PublicApi.TicketTypes.Responses;

namespace Eventix.Modules.Events.PublicApi
{
    public interface IEventsApi
    {
        Task<TicketTypeResponse?> GetTicketTypeAsync(Guid ticketTypeId, CancellationToken cancellationToken = default);
    }
}