using Eventix.Modules.Attendance.Domain.Tickets.Entities;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Attendance.Domain.Tickets.Errors
{
    public static class TicketErrors
    {
        public static Error NotFound(string code) =>
            Error.NotFound("Tickets.NotFound", $"The ticket with the code {code} was not found");

        public static readonly Error InvalidAttendeeId = Error.Problem(
           "Ticket.InvalidAttendeeId",
           "Attendee ID cannot be empty");

        public static readonly Error InvalidOrderId = Error.Problem(
            "Ticket.InvalidOrderId",
            "Order ID cannot be empty");

        public static readonly Error InvalidEventId = Error.Problem(
            "Ticket.InvalidEventId",
            "Event ID cannot be empty");

        public static readonly Error CodeIsRequired = Error.Problem(
            "Ticket.CodeIsRequired",
            "Ticket code is required");

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

        public static readonly Error InvalidCheckIn = Error.Problem(
            "Tickets.InvalidCheckIn",
            "The ticket check in was invalid");

        public static readonly Error DuplicateCheckIn = Error.Problem(
            "Tickets.DuplicateCheckIn",
            "The ticket was already checked in");
    }
}