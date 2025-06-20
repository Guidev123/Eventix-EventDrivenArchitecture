using Eventix.Modules.Ticketing.Domain.Orders.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Ticketing.Domain.Orders.ValueObjects
{
    public sealed record Quantity : ValueObject
    {
        public Quantity(decimal value)
        {
            Value = value;
            Validate();
        }

        private Quantity()
        { }

        public decimal Value { get; }

        public static implicit operator Quantity(decimal quantity)
            => new(quantity);

        protected override void Validate()
        {
            AssertionConcern.EnsureGreaterThan(Value, 0, OrderErrors.QuantityShouldBeGreaterThan0.Description);
        }
    }
}