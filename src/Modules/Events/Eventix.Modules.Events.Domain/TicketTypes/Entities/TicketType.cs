using Eventix.Modules.Events.Domain.Shared;
using Eventix.Modules.Events.Domain.TicketTypes.DomainEvents;
using Eventix.Modules.Events.Domain.TicketTypes.ValueObjects;

namespace Eventix.Modules.Events.Domain.TicketTypes.Entities
{
    public sealed class TicketType : Entity
    {
        private TicketType(Guid eventId, string name, decimal price, string currency, int quantity)
        {
            EventId = eventId;
            Specification = (name, quantity);
            Price = (price, currency);
        }

        private TicketType()
        { }

        public Guid EventId { get; private set; }
        public TicketTypeSpecification Specification { get; private set; } = null!;
        public Money Price { get; private set; } = null!;

        public static TicketType Create(Guid eventId, string name, decimal price, string currency, int quantity)
            => new(eventId, name, price, currency, quantity);

        public void UpdatePrice(decimal price)
        {
            if (Price.Amount == price) return;
            Price = new Money(price, Price.Currency);

            Raise(new TicketTypePriceChangedDomainEvent(Id, Price.Amount));
        }
    }
}