using Eventix.Shared.Domain.Responses;
using Eventix.Shared.Domain.ValueObjects;

namespace Eventix.Modules.Ticketing.Domain.Events.Errors
{
    public static class TicketTypeErrors
    {
        public static Error NotFound(Guid ticketTypeId) =>
              Error.NotFound("TicketTypes.NotFound", $"The ticket type with the identifier {ticketTypeId} was not found");

        public static Error UnableToUpdate(string name, Guid id) =>
            Error.Problem("TicketTypes.UnableToUpdate", $"Unable to update the ticket type {name} with identifier {id}");

        public static Error NotEnoughQuantity(decimal availableQuantity) =>
            Error.Problem(
                "TicketTypes.NotEnoughQuantity",
                $"The ticket type has {availableQuantity} quantity available");

        public static readonly Error EventIdIsRequired = Error.Problem(
           "TicketType.EventIdIsRequired",
           "The event identifier is required.");

        public static readonly Error SpecificationIsRequired = Error.Problem(
            "TicketType.SpecificationIsRequired",
            "The ticket specification is required.");

        public static readonly Error NameIsRequired = Error.Problem(
            "TicketType.NameIsRequired",
            "The ticket name is required.");

        public static readonly Error NameLengthInvalid = Error.Problem(
            "TicketType.NameLengthInvalid",
            "The ticket name must be between 3 and 100 characters.");

        public static readonly Error QuantityMustBeGreaterThanZero = Error.Problem(
            "TicketType.QuantityMustBeGreaterThanZero",
            "The ticket quantity must be greater than zero.");

        public static readonly Error AvailableQuantityMustBeGreaterThanOrEqualZero = Error.Problem(
            "TicketType.QuantityMustBeGreaterThanOrEqualZero",
            "The ticket quantity must be greater than or equal zero.");

        public static readonly Error PriceIsRequired = Error.Problem(
            "TicketType.PriceIsRequired",
            "The ticket price is required.");

        public static readonly Error FailToCreateTicket = Error.Problem(
            "TicketTypes.Create",
            "An error occurred while creating the ticket type.");

        public static readonly Error NameTooLong = Error.Problem(
            "Tickets.NameTooLong",
            "Ticket type name must not exceed 100 characters.");

        public static readonly Error CurrencyTooShort = Error.Problem(
            "Tickets.CurrencyTooShort",
            $"Currency length must be greater than {Money.CURRENCY_CODE_LEN} characters.");

        public static readonly Error CurrencyTooLong = Error.Problem(
            "Tickets.CurrencyTooLong",
            $"Currency length must be less than {Money.CURRENCY_CODE_LEN} characters.");

        public static readonly Error InvalidTicketTypeId = Error.Problem(
           "TicketType.InvalidTicketTypeId",
           "Ticket type ID cannot be empty");

        public static readonly Error InvalidEventId = Error.Problem(
            "TicketType.InvalidEventId",
            "Event ID cannot be empty");

        public static readonly Error PriceMustBeGreaterThanZero = Error.Problem(
            "TicketType.PriceMustBeGreaterThanZero",
            "Ticket type price must be greater than zero");

        public static readonly Error CurrencyIsRequired = Error.Problem(
            "TicketType.CurrencyIsRequired",
            "Currency is required");
    }
}