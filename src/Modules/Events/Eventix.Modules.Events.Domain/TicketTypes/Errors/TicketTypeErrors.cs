using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Domain.TicketTypes.Errors
{
    public static class TicketTypeErrors
    {
        public static Error NotFound(Guid ticketTypeId) =>
            Error.NotFound("TicketTypes.NotFound", $"The ticket type with the identifier {ticketTypeId} was not found");

        public static Error UnableToUpdate(string name, Guid id) =>
            Error.Problem("TicketTypes.UnableToUpdate", $"Unable to update the ticket type {name} with identifier {id}");
    }
}