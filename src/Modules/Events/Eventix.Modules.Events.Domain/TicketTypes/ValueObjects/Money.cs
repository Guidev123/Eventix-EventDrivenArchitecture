using Eventix.Modules.Events.Domain.TicketTypes.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Events.Domain.TicketTypes.ValueObjects
{
    public sealed record Money : ValueObject
    {
        public Money(decimal price, string currency)
        {
            Amount = price;
            Currency = currency;
            Validate();
        }
        private Money()
        {
        }

        public decimal Amount { get; }
        public string Currency { get; } = string.Empty;

        public static implicit operator Money((decimal price, string currency) value)
            => new(value.price, value.currency);

        public override string ToString() => $"{Amount} {Currency}";

        protected override void Validate()
        {
            AssertionConcern.EnsureGreaterThan(Amount, 0, TicketTypeErrors.PriceMustBeGreaterThanZero.Description);
            AssertionConcern.EnsureNotEmpty(Currency, TicketTypeErrors.CurrencyIsRequired.Description);
            AssertionConcern.EnsureLengthInRange(Currency, 2, 5, TicketTypeErrors.CurrencyLengthInvalid.Description);
        }
    }
}