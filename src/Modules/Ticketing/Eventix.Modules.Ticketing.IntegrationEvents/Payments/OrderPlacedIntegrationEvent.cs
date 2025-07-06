using Eventix.Shared.Application.EventBus;

namespace Eventix.Modules.Ticketing.IntegrationEvents.Payments
{
    public sealed record OrderPlacedIntegrationEvent : IntegrationEvent
    {
        public OrderPlacedIntegrationEvent(
            Guid id,
            DateTime occurredOnUtc,
            Guid orderId,
            decimal totalPrice,
            string currency
            ) : base(id, occurredOnUtc)
        {
            OrderId = orderId;
            TotalPrice = totalPrice;
            Currency = currency;
        }

        public Guid OrderId { get; init; }
        public decimal TotalPrice { get; init; }
        public string Currency { get; init; }
    }
}