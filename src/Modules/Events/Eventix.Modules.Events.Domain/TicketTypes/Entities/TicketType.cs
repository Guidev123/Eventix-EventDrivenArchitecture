using Eventix.Modules.Events.Domain.TicketTypes.DomainEvents;
using Eventix.Modules.Events.Domain.TicketTypes.Errors;
using Eventix.Modules.Events.Domain.TicketTypes.ValueObjects;
using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Domain.ValueObjects;

namespace Eventix.Modules.Events.Domain.TicketTypes.Entities
{
    public sealed class TicketType : Entity, IAggregateRoot
    {
        private TicketType(Guid eventId, string name, decimal price, string currency, decimal quantity)
        {
            EventId = eventId;
            Specification = (name, quantity);
            Price = (price, currency);
            Validate();
        }

        private TicketType()
        { }

        public Guid EventId { get; private set; }
        public TicketTypeSpecification Specification { get; private set; } = null!;
        public Money Price { get; private set; } = null!;

        public static TicketType Create(Guid eventId, string name, decimal price, string currency, decimal quantity)
            => new(eventId, name, price, currency, quantity);

        public void UpdatePrice(decimal price)
        {
            if (Price.Amount == price) return;
            Price = new Money(price, Price.Currency);

            Raise(new TicketTypePriceChangedDomainEvent(Id, Price.Amount));
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureTrue(EventId != Guid.Empty, TicketTypeErrors.EventIdIsRequired.Description);
            AssertionConcern.EnsureNotNull(Specification, TicketTypeErrors.SpecificationIsRequired.Description);
            AssertionConcern.EnsureNotNull(Price, TicketTypeErrors.PriceIsRequired.Description);
        }
    }
}