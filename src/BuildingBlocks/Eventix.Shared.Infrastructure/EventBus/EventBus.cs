using Eventix.Shared.Application.EventBus;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text.Json;
using ExchangeType = RabbitMQ.Client.ExchangeType;

namespace Eventix.Shared.Infrastructure.EventBus
{
    internal sealed class EventBus : IEventBus
    {
        private const int RETRY_COUNT = 5;
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection = default!;
        private IChannel _channel = default!;
        private readonly string _brokerUrl;

        public EventBus(string brokerUrl)
        {
            _brokerUrl = brokerUrl;
            _connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(_brokerUrl),
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            TryConnect().GetAwaiter().GetResult();
        }

        public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
             where T : IntegrationEvent
        {
            await EnsureConnectedAsync();

            var exchangeName = typeof(T).FullName;
            if (string.IsNullOrEmpty(exchangeName))
                throw new ArgumentException("Integration event type name cannot be null or empty.", nameof(T));

            await _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic, true, cancellationToken: cancellationToken);

            var body = JsonSerializer.SerializeToUtf8Bytes(integrationEvent);

            await _channel.BasicPublishAsync(exchangeName, string.Empty, body, cancellationToken);
        }

        public async Task SubscribeAsync<T>(
            string queueName,
            Func<T, Task> onMessage,
            CancellationToken cancellationToken = default
            ) where T : IntegrationEvent
        {
            await EnsureConnectedAsync();

            var exchangeName = typeof(T).FullName;
            if (string.IsNullOrEmpty(exchangeName))
                throw new ArgumentException("Integration event type name cannot be null or empty.", nameof(T));

            await Task.WhenAll(
                _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic, true, cancellationToken: cancellationToken),
                _channel.QueueDeclareAsync(queueName, true, false, false, cancellationToken: cancellationToken),
                _channel.QueueBindAsync(queueName, exchangeName, string.Empty, cancellationToken: cancellationToken)
            );

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, @event) =>
            {
                var body = @event.Body.ToArray();
                var message = JsonSerializer.Deserialize<T>(body);

                if (message is not null)
                    await onMessage(message);
            };

            await _channel.BasicConsumeAsync(queueName, true, consumer, cancellationToken: cancellationToken);
        }

        private async Task TryConnect()
        {
            var policy = Policy
                .Handle<BrokerUnreachableException>()
                .Or<IOException>()
                .Or<Exception>()
                .WaitAndRetryAsync(RETRY_COUNT, retry =>
                    TimeSpan.FromSeconds(Math.Pow(2, retry))
                );

            await policy.ExecuteAsync(async () =>
            {
                _connection = await _connectionFactory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
            });
        }

        private async Task EnsureConnectedAsync()
        {
            if (!IsConnected)
                await TryConnect();
        }

        private bool IsConnected
            => _connection is not null && _connection.IsOpen && _channel is not null && _channel.IsOpen;

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}