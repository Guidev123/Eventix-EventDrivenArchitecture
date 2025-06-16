using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Domain.Events.Errors
{
    public static class TicketingErrors
    {
        public static Error TicketTypeNotFound(Guid ticketTypeId) => Error.NotFound(
            "TicketType.TicketTypeNotFound",
            $"Could not find any customer with this ID: {ticketTypeId}");
    }
}