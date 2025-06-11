using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Domain.TicketTypes.Errors
{
    public static class TicketTypeErrors
    {
        public static Error NotFound(Guid ticketTypeId) =>
            Error.NotFound("TicketTypes.NotFound", $"The ticket type with the identifier {ticketTypeId} was not found");

        public static Error UnableToUpdate(string name, Guid id) =>
            Error.Problem("TicketTypes.UnableToUpdate", $"Unable to update the ticket type {name} with identifier {id}");

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

        public static readonly Error PriceIsRequired = Error.Problem(
            "TicketType.PriceIsRequired",
            "The ticket price is required.");

        public static readonly Error PriceMustBeGreaterThanZero = Error.Problem(
            "TicketType.PriceMustBeGreaterThanZero",
            "The ticket price must be greater than zero.");

        public static readonly Error CurrencyIsRequired = Error.Problem(
            "TicketType.CurrencyIsRequired",
            "The ticket currency is required.");

        public static readonly Error CurrencyLengthInvalid = Error.Problem(
            "TicketType.CurrencyLengthInvalid",
            "The ticket currency must be between 2 and 5 characters.");

        public static readonly Error FailToCreateTicket = Error.Problem(
            "TicketTypes.Create",
            "An error occurred while creating the ticket type.");
    }
}