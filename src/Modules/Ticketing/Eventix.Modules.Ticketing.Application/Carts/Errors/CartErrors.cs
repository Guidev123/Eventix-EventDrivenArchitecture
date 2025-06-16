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
    }
}