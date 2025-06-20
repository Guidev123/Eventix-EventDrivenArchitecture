using Eventix.Modules.Ticketing.Domain.Tickets.Entities;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Domain.Tickets.Errors
{
    public static class TicketErrors
    {
        public static Error NotFound(Guid ticketId) =>
            Error.NotFound("Tickets.NotFound", $"The ticket with the identifier {ticketId} was not found");

        public static Error NotFound(string code) =>
            Error.NotFound("Tickets.NotFound", $"The ticket with the code {code} was not found");

        public static readonly Error FailToPersistArchiveTickets = Error.Problem(
            "Tickets.FailToPersistArchiveTickets",
            "Fail to archive tickets");

        public static readonly Error FailToCreateTickets = Error.Problem(
            "Tickets.FailToCreateTickets",
            "Something has failed to create tickets");

        public static readonly Error FailToCreateTicket = Error.Problem(
            "Ticket.FailToCreateTicket",
            "Something has failed during ticket creation");

        public static readonly Error InvalidTicketData = Error.Problem(
            "Ticket.InvalidTicketData",
            "Invalid ticket data provided");

        public static readonly Error InvalidCustomerId = Error.Problem(
           "Ticket.InvalidCustomerId",
           "Customer ID cannot be empty");

        public static readonly Error InvalidOrderId = Error.Problem(
            "Ticket.InvalidOrderId",
            "Order ID cannot be empty");

        public static readonly Error InvalidEventId = Error.Problem(
            "Ticket.InvalidEventId",
            "Event ID cannot be empty");

        public static readonly Error InvalidTicketTypeId = Error.Problem(
            "Ticket.InvalidTicketTypeId",
            "Ticket type ID cannot be empty");

        public static readonly Error CodeIsRequired = Error.Problem(
            "Ticket.CodeIsRequired",
            "Ticket code is required");

        public static readonly Error InvalidCreatedDate = Error.Problem(
            "Ticket.InvalidCreatedDate",
            "Created date cannot be default value");

        public static readonly Error CodeCannotBeEmpty = Error.Problem(
            "TicketCode.CodeCannotBeEmpty",
            "Ticket code cannot be empty");

        public static readonly Error InvalidCodeLength = Error.Problem(
            "TicketCode.InvalidCodeLength",
            $"Ticket code must be exactly {Ticket.TICKET_CODE_LEN} characters long");

        public static readonly Error InvalidCodeFormat = Error.Problem(
            "TicketCode.InvalidCodeFormat",
            "Ticket code format is invalid");

        public static readonly Error InvalidCodePrefix = Error.Problem(
            "TicketCode.InvalidCodePrefix",
            $"Ticket code must start with '{Ticket.TICKET_CODE_PREFIX}'");
    }
}