namespace Eventix.Shared.Infrastructure.Http
{
    public static class HttpMessageHandlerFactory
    {
        private static readonly TimeSpan _pooledConnectionLifeTime = TimeSpan.FromSeconds(5);

        public static SocketsHttpHandler CreateSocketsHttpHandler()
            => new()
            { PooledConnectionLifetime = _pooledConnectionLifeTime };
    }
}