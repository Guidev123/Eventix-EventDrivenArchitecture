using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Domain.TicketTypes.Errors
{
    public static class TicketTypeErrors
    {
        public static Error NotFound(Guid ticketTypeId) =>
            Error.NotFound("TicketTypes.NotFound", $"The ticket type with the identifier {ticketTypeId} was not found");
    }
}