using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Domain.TicketTypes.Entities
{
    public sealed class TicketType : Entity
    {
        private TicketType(Guid eventId, string name, decimal price, string currency, decimal quantity)
        {
            EventId = eventId;
            Name = name;
            Price = price;
            Currency = currency;
            Quantity = quantity;
        }

        private TicketType()
        { }

        public Guid EventId { get; private set; }
        public string Name { get; private set; } = null!;
        public decimal Price { get; private set; }
        public string Currency { get; private set; } = null!;
        public decimal Quantity { get; private set; }

        public static TicketType Create(Guid eventId, string name, decimal price, string currency, decimal quantity)
            => new(eventId, name, price, currency, quantity);

        public void UpdatePrice(decimal price)
        {
            if (Price == price) return;
            Price = price;

            Raise(new TicketTypePriceChangedDomainEvent(Id, Price));
        }
    }
}