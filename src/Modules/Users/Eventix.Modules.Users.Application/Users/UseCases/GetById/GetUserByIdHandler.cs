using Eventix.Modules.Users.Application.Users.Mappers;
using Eventix.Modules.Users.Domain.Users.Errors;
using Eventix.Modules.Users.Domain.Users.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Users.Application.Users.UseCases.GetById
{
    internal sealed class GetUserByIdHandler(IUserRepository userRepository) : IQueryHandler<GetUserByIdQuery, GetUserByIdResponse>
    {
        public async Task<Result<GetUserByIdResponse>> ExecuteAsync(GetUserByIdQuery request, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken).ConfigureAwait(false);

            return user is null
                ? Result.Failure<GetUserByIdResponse>(UserErrors.NotFound(request.UserId))
                : Result.Success(user.MapFromUser());
        }
    }
}