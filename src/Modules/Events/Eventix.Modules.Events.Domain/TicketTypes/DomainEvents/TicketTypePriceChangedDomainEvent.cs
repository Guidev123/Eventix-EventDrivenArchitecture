using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Events.Domain.TicketTypes.DomainEvents
{
    public record TicketTypePriceChangedDomainEvent : DomainEvent
    {
        public TicketTypePriceChangedDomainEvent(Guid ticketId, decimal price) : base(ticketId)
        {
            TicketId = ticketId;
            Price = price;
        }

        public Guid TicketId { get; }
        public decimal Price { get; }
    }
}