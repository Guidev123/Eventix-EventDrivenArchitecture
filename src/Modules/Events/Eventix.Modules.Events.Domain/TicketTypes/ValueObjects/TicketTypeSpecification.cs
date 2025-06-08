using Eventix.Modules.Events.Domain.TicketTypes.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Events.Domain.TicketTypes.ValueObjects
{
    public record TicketTypeSpecification : ValueObject
    {
        public TicketTypeSpecification(string name, decimal quantity)
        {
            Name = name;
            Quantity = quantity;
            Validate();
        }

        public string Name { get; }
        public decimal Quantity { get; }

        public static implicit operator TicketTypeSpecification((string name, decimal quantity) value)
            => new(value.name, value.quantity);

        public override string ToString() => $"{Name} ({Quantity})";

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Name, TicketTypeErrors.NameIsRequired.Description);
            AssertionConcern.EnsureLengthInRange(Name, 3, 100, TicketTypeErrors.NameLengthInvalid.Description);
            AssertionConcern.EnsureTrue(Quantity > 0, TicketTypeErrors.QuantityMustBeGreaterThanZero.Description);
        }
    }
}