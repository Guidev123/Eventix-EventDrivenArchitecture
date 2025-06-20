using Eventix.Shared.Domain.Responses;

namespace Eventix.Shared.Domain.ValueObjects.Errors
{
    public static class ValueObjectErrors
    {
        public static readonly Error PriceMustBeGreaterThanZero = Error.Problem(
            "Money.PriceMustBeGreaterThanZero",
            "The ticket price must be greater than zero.");

        public static readonly Error CurrencyIsRequired = Error.Problem(
            "Money.CurrencyIsRequired",
            "The ticket currency is required.");

        public static readonly Error CurrencyLengthInvalid = Error.Problem(
            "Money.CurrencyLengthInvalid",
            "The ticket currency must be between 2 and 5 characters.");

        public static readonly Error InvalidPaymentData = Error.Problem(
            "Money.InvalidPaymentData",
            "Invalid payment data provided");

        public static readonly Error InvalidEmailFormart = Error.Failure(
            "Email.InvalidEmailFormat",
            "Invalid email format");

        public static readonly Error NameMustBeNotEmpty = Error.Failure(
             "Name.UserNameMustBeNotEmpty",
             "Name name must be not empty");

        public static Error NameLengthMustNotExceedTheLimitCharacters(int maxLength) => Error.Failure(
            "Name.UserNameLengthMustNotExceedTheLimitCharacters",
            $"User name length must not exceed {maxLength} characters");

        public static readonly Error InvalidCurrencyLength = Error.Problem(
            "Money.InvalidCurrencyLength",
            "Currency must be exactly 3 characters long");

        public static readonly Error InvalidCurrencyFormat = Error.Problem(
            "Money.InvalidCurrencyFormat",
            "Currency must contain only uppercase letters");
    }
}