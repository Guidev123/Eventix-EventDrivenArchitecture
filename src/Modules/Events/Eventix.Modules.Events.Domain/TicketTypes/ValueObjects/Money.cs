using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Events.Domain.TicketTypes.ValueObjects
{
    public sealed record Money : ValueObject
    {
        public const int MIN_CURRENCY_LENGTH = 2;
        public const int MAX_CURRENCY_LENGTH = 5;
        public const int CURRENCY_CODE_LEN = 3;
        public const string CURRENCY_CODE_PATTERN = @"^[A-Z]{3}$";
        public const decimal MINIMUM_AMOUNT = 0.01M;

        public Money(decimal ammount, string currency)
        {
            Amount = ammount;
            Currency = currency;
            Validate();
        }
        private Money()
        {
        }

        public decimal Amount { get; }
        public string Currency { get; } = string.Empty;

        public static implicit operator Money((decimal ammount, string currency) value)
            => new(value.ammount, value.currency);

        public override string ToString() => $"{Amount} {Currency}";

        protected override void Validate()
        {
            AssertionConcern.EnsureGreaterThan(
                Amount,
                MINIMUM_AMOUNT,
                EventErrors.PriceMustBeGreaterThanZero.Description);

            AssertionConcern.EnsureNotEmpty(
                Currency,
                EventErrors.CurrencyIsRequired.Description);

            AssertionConcern.EnsureLengthInRange(
                Currency,
                MIN_CURRENCY_LENGTH,
                MAX_CURRENCY_LENGTH,
                EventErrors.CurrencyLengthInvalid.Description);

            AssertionConcern.EnsureMatchesPattern(
                CURRENCY_CODE_PATTERN,
                Currency,
                EventErrors.InvalidPaymentData.Description);
        }
    }
}