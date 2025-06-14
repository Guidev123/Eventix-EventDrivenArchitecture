using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Domain.Errors
{
    public static class TicketingErrors
    {
        public static Error CartNotFound(Guid customerId) => Error.NotFound(
            "Carts.CartNotFound",
            $"Cart was not found, verify your customer ID: {customerId}");

        public static Error CustomerNotFound(Guid customerId) => Error.NotFound(
            "Customer.CustomerNotFound",
            $"Could not find any customer with this ID: {customerId}");

        public static Error TicketTypeNotFound(Guid ticketTypeId) => Error.NotFound(
            "TicketType.TicketTypeNotFound",
            $"Could not find any customer with this ID: {ticketTypeId}");

        public static readonly Error FailToAddItemToCart = Error.Problem(
            "Carts.FailToAddItemToCart",
            "Fail to add item to cart");
    }
}