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

        public static readonly Error CustomerIdIsRequired = Error.Problem(
            "Customers.CustomerIdIsRequired",
            "Customer ID must not be empty");

        public static readonly Error InvalidEmailFormat = Error.Problem(
            "Customers.InvalidEmailFormat",
            "Invalid e-mail format");

        public static readonly Error FirstNameLengthInvalid = Error.Problem(
            "Customers.FirstNameLengthInvalid",
            "First Name must be between 2 and 50 characters");

        public static readonly Error LastNameLengthInvalid = Error.Problem(
            "Customers.LastNameLengthInvalid",
            "Last Name must be between 2 and 50 characters");

        public static readonly Error CustomerIdCannotBeEmpty = Error.Problem(
            "Customers.CustomerIdCannotBeEmpty",
            "Customer ID cannot be empty");

        public static readonly Error FirstNameMaxLengthExceeded = Error.Problem(
            "Customers.FirstNameMaxLengthExceeded",
            "First name cannot exceed 50 characters");

        public static readonly Error LastNameMaxLengthExceeded = Error.Problem(
            "Customers.LastNameMaxLengthExceeded",
            "Last name cannot exceed 50 characters");
    }
}