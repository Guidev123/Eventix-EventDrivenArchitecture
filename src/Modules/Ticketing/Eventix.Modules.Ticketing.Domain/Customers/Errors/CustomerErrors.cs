using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Domain.Customers.Errors
{
    public static class CustomerErrors
    {
        public static Error NotFound(Guid customerId) => Error.NotFound(
            "Customers.NotFound",
            $"Could not find any customer with this ID: {customerId}");

        public static readonly Error AlreadyExists = Error.Problem(
            "Customers.UnableToCreateCustomer",
            "Unable to create customer");

        public static readonly Error SomethingHasFailedDuringPersistence = Error.Problem(
            "Customers.FailToPersistCustomer",
            "Fail to persist customer");

        public static readonly Error InvalidEmailFormart = Error.Failure(
           "Customers.InvalidEmailFormat",
           "Invalid email format");

        public static readonly Error CustomerNameMustBeNotEmpty = Error.Failure(
            "Customers.CustomerNameMustBeNotEmpty",
            "User name must be not empty");

        public static Error CustomerNameLengthMustNotExceedTheLimitCharacters(int maxLength) => Error.Failure(
            "Customers.CustomerNameLengthMustNotExceedTheLimitCharacters",
            $"User name length must not exceed {maxLength} characters");

        public static readonly Error EmailMustBeNotEmpty = Error.Failure(
            "Customers.EmailMustBeNotEmpty",
            "Email must be not empty");

        public static readonly Error NameMustBeNotEmpty = Error.Failure(
            "Customers.NameMustBeNotEmpty",
            "Name must be not empty");

        public static readonly Error FailToUpdate = Error.Failure(
            "Customers.FailToUpdate",
            "Fail to update customer properties");
    }
}