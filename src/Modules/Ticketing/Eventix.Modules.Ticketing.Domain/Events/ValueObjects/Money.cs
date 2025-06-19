using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Ticketing.Domain.Events.ValueObjects
{
    public sealed record Money : ValueObject
    {
        public static int MIN_CURRENCY_LENGTH = 2;
        public static int MAX_CURRENCY_LENGTH = 5;

        public Money(decimal price, string currency)
        {
            Amount = price;
            Currency = currency;
            Validate();
        }
        private Money()
        { }

        public decimal Amount { get; }
        public string Currency { get; } = string.Empty;

        public static implicit operator Money((decimal price, string currency) value)
            => new(value.price, value.currency);

        public override string ToString() => $"{Amount} {Currency}";

        protected override void Validate()
        {
            AssertionConcern.EnsureGreaterThan(Amount, 0, TicketTypeErrors.PriceMustBeGreaterThanZero.Description);
            AssertionConcern.EnsureNotEmpty(Currency, TicketTypeErrors.CurrencyIsRequired.Description);
            AssertionConcern.EnsureLengthInRange(Currency, MIN_CURRENCY_LENGTH, MAX_CURRENCY_LENGTH, TicketTypeErrors.CurrencyLengthInvalid.Description);
        }
    }
}