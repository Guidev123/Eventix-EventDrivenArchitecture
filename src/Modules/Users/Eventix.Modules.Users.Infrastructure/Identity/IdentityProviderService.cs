using Eventix.Modules.Users.Application.Abstractions.Identity.Dtos;
using Eventix.Modules.Users.Application.Abstractions.Identity.Errors;
using Eventix.Modules.Users.Application.Abstractions.Identity.Services;
using Eventix.Shared.Domain.Responses;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Eventix.Modules.Users.Infrastructure.Identity
{
    internal sealed class IdentityProviderService(KeyCloakClient keyCloakClient, ILogger<IdentityProviderService> logger) : IIdentityProviderService
    {
        private const string PASSWORD_CREDENTIAL_TYPE = "password";

        public async Task<Result<string>> RegisterAsync(UserDto userDto, CancellationToken cancellationToken = default)
        {
            var request = new UserRepresentationDto(
                userDto.Email,
                userDto.Email,
                userDto.FirstName,
                userDto.LastName,
                false,
                true,
                [new CredentialRepresentationDto(PASSWORD_CREDENTIAL_TYPE, userDto.Password, false)]
                );

            try
            {
                var identityId = await keyCloakClient.RegisterAsync(request, cancellationToken);

                return string.IsNullOrWhiteSpace(identityId)
                    ? Result.Failure<string>(Error.NullValue)
                    : Result.Success(identityId);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
            {
                logger.LogError(ex, "User registration failed");
                return Result.Failure<string>(IdentityProviderErrors.EmailIsNotUnique);
            }
        }
    }
}