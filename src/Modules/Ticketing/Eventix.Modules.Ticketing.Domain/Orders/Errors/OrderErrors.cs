using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Domain.Orders.Errors
{
    public static class OrderErrors
    {
        public static Error NotFound(Guid orderId) =>
            Error.NotFound("Orders.NotFound", $"The order with the identifier {orderId} was not found");

        public static readonly Error TicketsAlreadyIssues = Error.Problem(
            "Order.TicketsAlreadyIssued",
            "The tickets for this order were already issued");

        public static readonly Error TotalPriceMustBeNotNull = Error.Problem(
            "Order.TotalPriceMustBeNotNull",
            "Total price must be not null");

        public static readonly Error FailToCreateOrder = Error.Problem(
            "Order.FailToCreateOrder",
            "Something has failed during order creation");

        public static readonly Error UnableToPersistOrder = Error.Problem(
            "Order.UnableToPersistOrder",
            "Unable to persist order");

        public static readonly Error QuantityShouldBeGreaterThan0 = Error.Problem(
            "Order.QuantityShouldBeGreaterThan0",
            "Quantity should be greater than 0");

        public static readonly Error InvalidCustomerId = Error.Problem(
           "Order.InvalidCustomerId",
           "Customer ID cannot be empty");

        public static readonly Error InvalidStatus = Error.Problem(
            "Order.InvalidStatus",
            "Order status is invalid");

        public static readonly Error InvalidCreationDate = Error.Problem(
            "Order.InvalidCreationDate",
            "Order creation date cannot be default value");

        public static readonly Error InvalidOrderId = Error.Problem(
        "OrderItem.InvalidOrderId",
        "Order ID cannot be empty");

        public static readonly Error InvalidTicketTypeId = Error.Problem(
            "OrderItem.InvalidTicketTypeId",
            "Ticket type ID cannot be empty");

        public static readonly Error QuantityIsRequired = Error.Problem(
            "OrderItem.QuantityIsRequired",
            "Quantity is required");

        public static readonly Error UnitPriceIsRequired = Error.Problem(
            "OrderItem.UnitPriceIsRequired",
            "Unit price is required");

        public static readonly Error PriceIsRequired = Error.Problem(
            "OrderItem.PriceIsRequired",
            "Price is required");
    }
}