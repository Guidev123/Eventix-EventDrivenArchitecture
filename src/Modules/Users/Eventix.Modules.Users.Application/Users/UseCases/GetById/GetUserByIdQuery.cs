using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Users.Application.Users.UseCases.GetById
{
    public record GetUserByIdQuery(Guid UserId) : IQuery<GetUserByIdResponse>;
}