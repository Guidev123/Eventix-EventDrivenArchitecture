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

        public static readonly Error StartDateInPast = Error.Problem(
            "Date.StartDateInPast",
            "The event start date is in the past");

        public static readonly Error EndDatePrecedesStartDate = Error.Problem(
            "Date.EndDatePrecedesStartDate",
            "The event end date precedes the start date");

        public static readonly Error StartDateMustBeInFuture = Error.Problem(
           "Date.StartDateMustBeInFuture",
           "The start date must be in the future.");

        public static readonly Error EndDateMustBeAfterStartDate = Error.Problem(
            "Date.EndDateMustBeAfterStartDate",
            "The end date must be after the start date.");

        public static readonly Error StreetIsRequired = Error.Problem(
           "Location.StreetIsRequired",
           "The street is required.");

        public static readonly Error NumberIsRequired = Error.Problem(
            "Location.NumberIsRequired",
            "The number is required.");

        public static readonly Error NeighborhoodIsRequired = Error.Problem(
            "Location.NeighborhoodIsRequired",
            "The neighborhood is required.");

        public static readonly Error ZipCodeIsRequired = Error.Problem(
            "Location.ZipCodeIsRequired",
            "The zip code is required.");

        public static readonly Error CityIsRequired = Error.Problem(
            "Location.CityIsRequired",
            "The city is required.");

        public static readonly Error StateIsRequired = Error.Problem(
            "Location.StateIsRequired",
            "The state is required.");

        public static Error StreetTooShort(int min) => Error.Problem(
            "Location.StreetTooShort",
            $"The street must be at least {min} characters.");

        public static Error StreetTooLong(int max) => Error.Problem(
            "Location.StreetTooLong",
            $"The street must not exceed {max} characters.");

        public static Error NumberTooLong(int max) => Error.Problem(
            "Location.NumberTooLong",
            $"The number must not exceed {max} characters.");

        public static Error NeighborhoodTooLong(int max) => Error.Problem(
            "Location.NeighborhoodTooLong",
            $"The neighborhood must not exceed {max} characters.");

        public static Error ZipCodeTooLong(int max) => Error.Problem(
            "Location.ZipCodeTooLong",
            $"The zip code must not exceed {max} characters.");

        public static Error CityTooLong(int max) => Error.Problem(
            "Location.CityTooLong",
            $"The city must not exceed {max} characters.");

        public static Error StateInvalidLength(int exact) => Error.Problem(
            "Location.StateInvalidLength",
            $"The state must be exactly {exact} characters.");
    }
}