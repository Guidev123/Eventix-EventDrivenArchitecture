using Eventix.Modules.Users.Application.Abstractions.Identity.Dtos;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Users.Application.Abstractions.Identity.Services
{
    public interface IIdentityProviderService
    {
        Task<Result<string>> RegisterAsync(UserDto userDto, CancellationToken cancellationToken = default);
    }
}