using Eventix.Modules.Users.Application.Users.Mappers;
using Eventix.Modules.Users.Domain.Users.Errors;
using Eventix.Modules.Users.Domain.Users.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Users.Application.Users.UseCases.Register
{
    internal sealed class RegisterUserHandler(IUserRepository userRepository,
                                              IUnitOfWork unitOfWork) : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
    {
        public async Task<Result<RegisterUserResponse>> ExecuteAsync(RegisterUserCommand request, CancellationToken cancellationToken = default)
        {
            var user = request.MapToUser();

            var userAlreadyExists = await userRepository.ExistsAsync(user.Email.Address, cancellationToken).ConfigureAwait(false);
            if (userAlreadyExists)
                return Result.Failure<RegisterUserResponse>(UserErrors.UnableToCreateAccount);

            userRepository.Insert(user);

            var saveChanges = await unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

            return saveChanges ? Result.Success(new RegisterUserResponse(user.Id))
                : Result.Failure<RegisterUserResponse>(UserErrors.FailToCreate);
        }
    }
}