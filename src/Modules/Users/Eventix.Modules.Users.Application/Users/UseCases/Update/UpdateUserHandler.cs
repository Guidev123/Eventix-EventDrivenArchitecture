using Eventix.Modules.Users.Domain.Users.Entities;
using Eventix.Modules.Users.Domain.Users.Errors;
using Eventix.Modules.Users.Domain.Users.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Users.Application.Users.UseCases.Update
{
    internal sealed class UpdateUserHandler(IUserRepository userRepository,
                                            IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserCommand>
    {
        public async Task<Result> ExecuteAsync(UpdateUserCommand request, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken).ConfigureAwait(false);
            if (user is null)
                return Result.Failure(UserErrors.NotFound);

            UpdateProperties(user, request);

            var saveChanges = await unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges ? Result.Success() : Result.Failure(UserErrors.FailToUpdate);
        }

        private void UpdateProperties(User user, UpdateUserCommand command)
        {
            if (IsNameChange(command))
                UpdateUserName(user, command);

            if (IsEmailChange(command))
                UpdateUserEmail(user, command);
        }

        private void UpdateUserName(User user, UpdateUserCommand command)
        {
            if (!IsNameChange(command)) return;

            user.UpdateName(command.FirstName!, command.LastName!);
            userRepository.Update(user);
        }

        private void UpdateUserEmail(User user, UpdateUserCommand command)
        {
            if (!IsEmailChange(command)) return;

            user.UpdateEmail(command.Email!);
            userRepository.Update(user);
        }

        private static bool IsNameChange(UpdateUserCommand command)
            => !string.IsNullOrEmpty(command.FirstName) || !string.IsNullOrEmpty(command.LastName);

        private static bool IsEmailChange(UpdateUserCommand command)
            => command.Email is not null && !string.IsNullOrEmpty(command.Email.Trim());
    }
}