using Eventix.Modules.Ticketing.Application.Carts.Models;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Carts.Services
{
    public interface ICartService
    {
        Task<Result<Cart>> GetAsync(Guid customerId, CancellationToken cancellationToken = default);

        Task<Result> ClearAsync(Guid customerId, CancellationToken cancellationToken = default);

        Task<Result> AddItemAsync(Guid customerId, CartItem item, CancellationToken cancellationToken = default);

        Task<Result> RemoveItemAsync(Guid customerId, Guid ticketTypeId, CancellationToken cancellationToken = default);
    }
}