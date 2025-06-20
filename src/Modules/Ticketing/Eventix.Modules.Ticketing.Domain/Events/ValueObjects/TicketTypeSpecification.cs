using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Ticketing.Domain.Events.ValueObjects
{
    public record TicketTypeSpecification : ValueObject
    {
        public const int MIN_NAME_LENGTH = 3;
        public const int MAX_NAME_LENGTH = 100;
        public TicketTypeSpecification(string name, decimal quantity, decimal availableQuantity)
        {
            Name = name;
            Quantity = quantity;
            AvailableQuantity = availableQuantity;
            Validate();
        }

        private TicketTypeSpecification()
        { }

        public string Name { get; } = string.Empty;
        public decimal Quantity { get; }
        public decimal AvailableQuantity { get; }

        public static implicit operator TicketTypeSpecification((string name, decimal quantity, decimal availableQuantity) value)
            => new(value.name, value.quantity, value.availableQuantity);

        public override string ToString() => $"{Name} ({Quantity})";

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Name, TicketTypeErrors.NameIsRequired.Description);
            AssertionConcern.EnsureLengthInRange(Name, MIN_NAME_LENGTH, MAX_NAME_LENGTH, TicketTypeErrors.NameLengthInvalid.Description);
            AssertionConcern.EnsureTrue(Quantity > 0, TicketTypeErrors.QuantityMustBeGreaterThanZero.Description);
            AssertionConcern.EnsureTrue(AvailableQuantity >= 0, TicketTypeErrors.AvailableQuantityMustBeGreaterThanOrEqualZero.Description);
        }
    }
}