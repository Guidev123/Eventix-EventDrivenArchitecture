using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Users.Domain.Users.Errors
{
    public static class UserErrors
    {
        public static readonly Error UnableToCreateAccount = Error.Conflict(
            "Users.UnableToCreateAccount",
            "Unable to create account. Please check that the details are correct");

        public static readonly Error FailToCreate = Error.Problem(
            "Users.FailToCreate",
            "Fail to create user");

        public static readonly Error NotFound = Error.Problem(
            "Users.UserNotFound",
            "User not found");

        public static readonly Error FailToUpdate = Error.Problem(
            "Users.FailToUpdate",
            "Fail to update user");

        public static readonly Error EmailMustBeNotEmpty = Error.Failure(
            "Users.EmailMustBeNotEmpty",
            "Email must be not empty");

        public static readonly Error NameMustBeNotEmpty = Error.Failure(
            "Users.NameMustBeNotEmpty",
            "Name must be not empty");

        public static readonly Error AuditInfoMustBeNotEmpty = Error.Failure(
            "Users.AuditInfoMustBeNotEmpty",
            "Audit info must be not empty");

        public static readonly Error InvalidEmailFormart = Error.Failure(
            "Users.InvalidEmailFormat",
            "Invalid email format");

        public static readonly Error UserNameMustBeNotEmpty = Error.Failure(
            "Users.UserNameMustBeNotEmpty",
            "User name must be not empty");

        public static Error UserNameLengthMustNotExceedTheLimitCharacters(int maxLength) => Error.Failure(
            "Users.UserNameLengthMustNotExceedTheLimitCharacters",
            $"User name length must not exceed {maxLength} characters");

        public static readonly Error CreatedAtMustBeNotEmpty = Error.Failure(
            "Users.CreatedAtMustBeNotEmpty",
            "Created at must be not empty");

        public static readonly Error DeletedAtUtcMustBeNotNullWhenUserIsDeleted = Error.Failure(
            "Users.DeletedAtUtcMustBeNotNullWhenUserIsDeleted",
            "Deleted at UTC must be not null when user is deleted");

        public static readonly Error DeletedAtUtcMustBeNullWhenUserIsDeleted = Error.Failure(
            "Users.DeletedAtUtcMustBeNullWhenUserIsDeleted",
            "Deleted at UTC must be null when user is not deleted");

        public static readonly Error FirstNameIsRequired = Error.Problem(
            "Users.FirstNameIsRequired",
            "First name is required");

        public static readonly Error FirstNameTooLong = Error.Problem(
            "Users.FirstNameTooLong",
            "First name must not exceed 50 characters");

        public static readonly Error LastNameIsRequired = Error.Problem(
            "Users.LastNameIsRequired",
            "Last name is required");

        public static readonly Error LastNameTooLong = Error.Problem(
            "Users.LastNameTooLong",
            "Last name must not exceed 50 characters");

        public static readonly Error EmailIsRequired = Error.Problem(
            "Users.EmailIsRequired",
            "Email is required");

        public static readonly Error EmailInvalidFormat = Error.Problem(
            "Users.EmailInvalidFormat",
            "Invalid email format");

        public static readonly Error EmailTooLong = Error.Problem(
            "Users.EmailTooLong",
            "Email must not exceed 160 characters");

        public static readonly Error PasswordIsRequired = Error.Problem(
            "Users.PasswordIsRequired",
            "The password field cannot be empty");

        public static readonly Error PasswordTooShort = Error.Problem(
            "Users.PasswordTooShort",
            "The password must be at least 8 characters long");

        public static readonly Error PasswordMissingUpperCase = Error.Problem(
            "Users.PasswordMissingUpperCase",
            "The password must contain at least one uppercase letter");

        public static readonly Error PasswordMissingLowerCase = Error.Problem(
            "Users.PasswordMissingLowerCase",
            "The password must contain at least one lowercase letter");

        public static readonly Error PasswordMissingDigit = Error.Problem(
            "Users.PasswordMissingDigit",
            "The password must contain at least one digit");

        public static readonly Error PasswordMissingSpecialChar = Error.Problem(
            "Users.PasswordMissingSpecialChar",
            "The password must contain at least one special character (!@#$%^&* etc.)");

        public static readonly Error PasswordsDoNotMatch = Error.Problem(
            "Users.PasswordsDoNotMatch",
            "The passwords do not match");

        public static readonly Error UserIdEmpty = Error.Problem(
            "Users.UserIdEmpty",
            "Users ID cannot be empty");

        public static readonly Error InvalidEmailFormat = Error.Problem(
            "Users.InvalidEmailFormat",
            "Invalid email format");
    }
}