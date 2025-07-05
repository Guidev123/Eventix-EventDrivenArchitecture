using Eventix.Shared.Application.EventBus;

namespace Eventix.Modules.Events.IntegrationEvents.TicketTypes
{
    public sealed record TicketTypePriceChangedIntegrationEvent : IntegrationEvent
    {
        public TicketTypePriceChangedIntegrationEvent(Guid id, DateTime occurredOnUtc, Guid ticketTypeId, decimal price) : base(id, occurredOnUtc)
        {
            TicketTypeId = ticketTypeId;
            Price = price;
        }

        public Guid TicketTypeId { get; init; }
        public decimal Price { get; init; }
    }
}