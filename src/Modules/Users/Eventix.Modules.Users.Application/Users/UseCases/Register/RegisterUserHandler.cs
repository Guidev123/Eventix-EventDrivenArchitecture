using Eventix.Modules.Users.Application.Abstractions.Identity.Errors;
using Eventix.Modules.Users.Application.Abstractions.Identity.Services;
using Eventix.Modules.Users.Application.Users.Mappers;
using Eventix.Modules.Users.Domain.Users.Errors;
using Eventix.Modules.Users.Domain.Users.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Users.Application.Users.UseCases.Register
{
    internal sealed class RegisterUserHandler(IUserRepository userRepository,
                                              IIdentityProviderService identityProviderService) : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
    {
        public async Task<Result<RegisterUserResponse>> ExecuteAsync(RegisterUserCommand request, CancellationToken cancellationToken = default)
        {
            var userAlreadyExists = await userRepository.ExistsAsync(request.Email, cancellationToken).ConfigureAwait(false);
            if (userAlreadyExists)
                return Result.Failure<RegisterUserResponse>(UserErrors.UnableToCreateAccount);

            var identityProviderResult = await identityProviderService.RegisterAsync(
                new(request.Email, request.Password, request.FirstName, request.LastName), cancellationToken);

            if (identityProviderResult.IsFailure)
                return Result.Failure<RegisterUserResponse>(IdentityProviderErrors.EmailIsNotUnique);

            if (!Guid.TryParse(identityProviderResult.Value, out Guid identityId))
                return Result.Failure<RegisterUserResponse>(IdentityProviderErrors.FailToGetIdentityId);

            var user = request.MapToUser(identityId);

            userRepository.Insert(user);

            var saveChanges = await userRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

            return saveChanges ? Result.Success(new RegisterUserResponse(user.Id))
                : Result.Failure<RegisterUserResponse>(UserErrors.FailToCreate);
        }
    }
}