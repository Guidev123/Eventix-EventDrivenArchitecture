using Eventix.Shared.Application.EventBus;

namespace Eventix.Modules.Ticketing.IntegrationEvents.Orders
{
    public sealed record OrderCreatedIntegrationEvent : IntegrationEvent
    {
        public OrderCreatedIntegrationEvent(
            Guid id,
            DateTime occurredOnUtc,
            Guid orderId,
            decimal totalPrice,
            Guid customerId,
            DateTime createdAtUtc,
            List<OrderItemDto> orderItems) : base(id, occurredOnUtc)
        {
            OrderId = orderId;
            TotalPrice = totalPrice;
            CustomerId = customerId;
            CreatedAtUtc = createdAtUtc;
            OrderItems = orderItems;
        }

        public Guid OrderId { get; init; }
        public decimal TotalPrice { get; init; }
        public Guid CustomerId { get; init; }
        public DateTime CreatedAtUtc { get; init; }
        public List<OrderItemDto> OrderItems { get; init; } = [];

        public sealed record OrderItemDto(
            Guid OrderItemId,
            Guid OrderId,
            Guid TicketTypeId,
            decimal Quantity,
            decimal UnitPrice,
            decimal Price,
            string Currency
        );
    }
}