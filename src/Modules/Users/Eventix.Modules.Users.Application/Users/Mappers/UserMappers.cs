using Eventix.Modules.Users.Application.Users.UseCases.GetById;
using Eventix.Modules.Users.Application.Users.UseCases.Register;
using Eventix.Modules.Users.Domain.Users.Entities;

namespace Eventix.Modules.Users.Application.Users.Mappers
{
    public static class UserMappers
    {
        public static User MapToUser(this RegisterUserCommand command, Guid identityId)
            => User.Create(
                identityId,
                command.Email,
                command.FirstName,
                command.LastName);

        public static GetUserByIdResponse MapFromUser(this User user)
            => new(
                user.Id,
                user.Name.FirstName,
                user.Name.LastName,
                user.Email.Address
                );
    }
}