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
    }
}