using Eventix.Modules.Ticketing.Application.Carts.Errors;
using Eventix.Modules.Ticketing.Application.Carts.Models;
using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Shared.Application.Cache;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Carts.Services
{
    public sealed class CartService(ICacheService cacheService) : ICartService
    {
        private static readonly TimeSpan DefaultExpiration = TimeSpan.FromHours(1);

        public async Task<Result<Cart>> GetAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            var cacheKey = CreateCacheKey(customerId);

            var result = await cacheService.GetAsync<Cart>(cacheKey, cancellationToken);
            return result is not null ? result : new() { CustomerId = customerId };
        }

        public async Task<Result> ClearAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            var cacheKey = CreateCacheKey(customerId);

            var cart = new Cart() { CustomerId = customerId };

            await cacheService.SetAsync(cacheKey, cart, DefaultExpiration, cancellationToken);

            return Result.Success();
        }

        public async Task<Result> AddItemAsync(Guid customerId, CartItem item, CancellationToken cancellationToken = default)
        {
            var cacheKey = CreateCacheKey(customerId);

            var cart = (await GetAsync(customerId, cancellationToken)).Value;

            if (cart is null)
                return Result.Failure(CartErrors.NotFound(customerId));

            var existingItem = GetItem(cart.Items, item.TicketTypeId);

            if (existingItem is null)
            {
                cart.Items.Add(item);
            }
            else
            {
                cart.Items.Find(c => c.TicketTypeId == item.TicketTypeId)?.UpdateQuantity(item.Quantity);
            }

            await cacheService.SetAsync(cacheKey, cart, DefaultExpiration, cancellationToken);

            return Result.Success();
        }

        public async Task<Result> RemoveItemAsync(Guid customerId, Guid ticketTypeId, CancellationToken cancellationToken = default)
        {
            var cacheKey = CreateCacheKey(customerId);

            var cart = (await GetAsync(customerId, cancellationToken)).Value;

            if (cart is null)
                return Result.Failure(CartErrors.NotFound(customerId));

            var existingItem = GetItem(cart.Items, ticketTypeId);

            if (existingItem is null)
                return Result.Failure(CartErrors.NotFound(customerId));

            cart.Items.Remove(existingItem);

            await cacheService.SetAsync(cacheKey, cart, DefaultExpiration, cancellationToken);

            return Result.Success();
        }

        private static string CreateCacheKey(Guid customerId) => $"carts: {customerId}";

        private static CartItem? GetItem(List<CartItem> items, Guid ticketTypeId)
            => items.Find(c => c.TicketTypeId == ticketTypeId);
    }
}