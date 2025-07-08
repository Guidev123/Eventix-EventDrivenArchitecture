using EasyNetQ;
using Eventix.Shared.Application.EventBus;
using Polly;
using RabbitMQ.Client.Exceptions;
using IEventBus = Eventix.Shared.Application.EventBus.IEventBus;

namespace Eventix.Shared.Infrastructure.EventBus
{
    internal sealed class EventBus : IEventBus
    {
        private const int RETRY_COUNT = 3;
        private IBus _bus = default!;
        private IAdvancedBus _advancedBus = default!;
        private readonly string _connectionString;

        public EventBus(string connectionString)
        {
            _connectionString = connectionString;
            TryConnect();
        }

        public bool IsConnected => _bus?.Advanced.IsConnected ?? false;
        public IAdvancedBus AdvancedBus => _bus.Advanced;

        public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
            where T : IntegrationEvent
            => await TryConnect().PubSub.PublishAsync(
                integrationEvent,
                cancellationToken
            );

        public async Task SubscribeAsync<T>(
            string subscriptionId,
            Func<T, Task> onMessage,
            CancellationToken cancellationToken = default
            ) where T : IntegrationEvent
            => await TryConnect().PubSub.SubscribeAsync(
                subscriptionId,
                onMessage,
                cancellationToken: cancellationToken
            );

        private IBus TryConnect()
        {
            if (IsConnected) return _bus;

            var policy = Policy.Handle<EasyNetQException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(RETRY_COUNT, retry => TimeSpan.FromSeconds(Math.Pow(2, retry)));

            policy.Execute(() =>
            {
                _bus = RabbitHutch.CreateBus(_connectionString);
                _advancedBus = _bus.Advanced;
                _advancedBus.Disconnected += OnDisconnect!;
            });

            return _bus;
        }

        private void OnDisconnect(object x, EventArgs y)
        {
            var policy = Policy.Handle<EasyNetQException>()
                .Or<BrokerUnreachableException>()
                .RetryForever();

            policy.Execute(TryConnect);
        }

        public void Dispose() => _bus?.Dispose();
    }
}