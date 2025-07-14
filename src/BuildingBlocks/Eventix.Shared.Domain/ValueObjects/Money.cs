using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Domain.ValueObjects.Errors;

namespace Eventix.Shared.Domain.ValueObjects
{
    public sealed record Money : ValueObject
    {
        public const int CURRENCY_CODE_LEN = 3;
        public const string CURRENCY_CODE_PATTERN = @"^[A-Z]{3}$";
        public const decimal MINIMUM_AMOUNT = 0.01M;

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
            AssertionConcern.EnsureGreaterThan(
                Amount,
                MINIMUM_AMOUNT,
                ValueObjectErrors.PriceMustBeGreaterThanZero.Description);

            AssertionConcern.EnsureNotEmpty(
                Currency,
                ValueObjectErrors.CurrencyIsRequired.Description);

            AssertionConcern.EnsureLengthInRange(
                Currency,
                CURRENCY_CODE_LEN,
                CURRENCY_CODE_LEN,
                ValueObjectErrors.CurrencyLengthInvalid.Description);

            AssertionConcern.EnsureMatchesPattern(
                CURRENCY_CODE_PATTERN,
                Currency,
                ValueObjectErrors.InvalidPaymentData.Description);
        }
    }
}