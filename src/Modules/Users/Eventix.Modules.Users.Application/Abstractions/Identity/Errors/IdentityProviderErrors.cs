using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Users.Application.Abstractions.Identity.Errors
{
    public static class IdentityProviderErrors
    {
        public static readonly Error EmailIsNotUnique = Error.Conflict(
            "IdentityProvider.EmailIsNotUnique",
            "The specified E-mail is not unique");

        public static readonly Error FailToGetIdentityId = Error.Problem(
            "IdentityProvider.FailToGetIdentityId",
            "Something failed to get identity Id");
    }
}