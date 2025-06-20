using Eventix.Modules.Ticketing.Domain.Events.DomainEvents;
using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Modules.Ticketing.Domain.Events.ValueObjects;
using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Domain.Responses;
using Eventix.Shared.Domain.ValueObjects;

namespace Eventix.Modules.Ticketing.Domain.Events.Entities
{
    public sealed class TicketType : Entity, IAggregateRoot
    {
        private TicketType(Guid eventId, string name, decimal price, string currency, decimal quantity, decimal availableQuantity)
        {
            EventId = eventId;
            Specification = (name, quantity, availableQuantity);
            Price = (price, currency);
            Validate();
        }

        private TicketType()
        { }

        public Guid EventId { get; private set; }
        public TicketTypeSpecification Specification { get; private set; } = null!;
        public Money Price { get; private set; } = null!;

        public static TicketType Create(Guid eventId, string name, decimal price, string currency, decimal quantity)
            => new(eventId, name, price, currency, quantity, quantity);

        public void UpdatePrice(decimal price)
        {
            if (Price.Amount == price) return;
            Price = new Money(price, Price.Currency);
        }

        public Result UpdateQuantity(decimal quantity)
        {
            var currentAvailableQuantity = Specification.AvailableQuantity;

            if (currentAvailableQuantity < quantity)
                return Result.Failure(TicketTypeErrors.NotEnoughQuantity(currentAvailableQuantity));

            var newAvailableQuantity = currentAvailableQuantity -= quantity;
            Specification = (Specification.Name, Specification.Quantity, newAvailableQuantity);

            if (Specification.AvailableQuantity == 0)
                Raise(new TicketTypeSoldOutDomainEvent(Id));

            return Result.Success();
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureTrue(EventId != Guid.Empty, TicketTypeErrors.EventIdIsRequired.Description);
            AssertionConcern.EnsureNotNull(Specification, TicketTypeErrors.SpecificationIsRequired.Description);
            AssertionConcern.EnsureNotNull(Price, TicketTypeErrors.PriceIsRequired.Description);
        }
    }
}