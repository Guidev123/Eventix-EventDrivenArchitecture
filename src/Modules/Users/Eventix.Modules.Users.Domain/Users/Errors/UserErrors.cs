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
    }
}