using Microsoft.Extensions.Caching.Distributed;

namespace Eventix.Shared.Infrastructure.Cache
{
    internal static class CacheOptions
    {
        private static readonly int _defaultExpirationTimeInMinutes = 2;

        private static DistributedCacheEntryOptions DefaultExpiration
            => new()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_defaultExpirationTimeInMinutes)
            };

        public static DistributedCacheEntryOptions Create(TimeSpan? expiration)
            => expiration is not null ?
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration }
                : DefaultExpiration;
    }
}