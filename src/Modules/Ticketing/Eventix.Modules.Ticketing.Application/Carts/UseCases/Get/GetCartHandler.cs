using Eventix.Modules.Ticketing.Application.Carts.Models;
using Eventix.Modules.Ticketing.Application.Carts.Services;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Carts.UseCases.Get
{
    public sealed class GetCartHandler(ICartService cartService) : IQueryHandler<GetCartQuery, Cart>
    {
        public async Task<Result<Cart>> ExecuteAsync(GetCartQuery request, CancellationToken cancellationToken = default)
            => await cartService.GetAsync(request.CustomerId, cancellationToken);
    }
}