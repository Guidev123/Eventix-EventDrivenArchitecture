using Eventix.Modules.Users.Domain.Users.Entities;
using Eventix.Modules.Users.Domain.Users.Errors;
using Eventix.Modules.Users.Domain.Users.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Users.Application.Users.UseCases.Update
{
    internal sealed class UpdateUserHandler(IUserRepository userRepository) : ICommandHandler<UpdateUserCommand>
    {
        public async Task<Result> ExecuteAsync(UpdateUserCommand request, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken).ConfigureAwait(false);
            if (user is null)
                return Result.Failure(UserErrors.NotFound(request.UserId));

            UpdateUserName(user, request);

            var saveChanges = await userRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges ? Result.Success() : Result.Failure(UserErrors.FailToUpdate);
        }

        private void UpdateUserName(User user, UpdateUserCommand command)
        {
            user.UpdateName(command.FirstName, command.LastName);
            userRepository.Update(user);
        }
    }
}