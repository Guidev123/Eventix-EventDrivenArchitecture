using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Carts.Errors
{
    public static class CartErrors
    {
        public static Error NotFound(Guid customerId) => Error.NotFound(
            "Carts.CartNotFound",
            $"Cart was not found, verify your customer ID: {customerId}");

        public static readonly Error FailToAddItemToCart = Error.Problem(
            "Carts.FailToAddItemToCart",
            "Fail to add item to cart");

        public static readonly Error CustomerIdIsRequired = Error.Problem(
            "Orders.CustomerIdIsRequired",
            "Customer ID must not be empty");

        public static readonly Error TicketTypeIdIsRequired = Error.Problem(
            "Orders.TicketTypeIdIsRequired",
            "Ticket Type ID must not be empty");

        public static Error QuantityMustBeAtLeast(int min) =>
            Error.Problem(
                "Orders.QuantityMustBeAtLeast",
                $"Quantity must be greater than or equal to {min}");
    }
}