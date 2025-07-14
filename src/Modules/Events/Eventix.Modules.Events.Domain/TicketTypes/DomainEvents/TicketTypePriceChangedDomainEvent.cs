using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Events.Domain.TicketTypes.DomainEvents
{
    public record TicketTypePriceChangedDomainEvent : DomainEvent
    {
        public TicketTypePriceChangedDomainEvent(Guid ticketId, decimal price) : base(ticketId)
        {
            TicketTypeId = ticketId;
            Price = price;
        }

        public Guid TicketTypeId { get; }
        public decimal Price { get; }
    }
}