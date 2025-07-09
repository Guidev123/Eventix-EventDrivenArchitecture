using Eventix.Shared.Application.EventBus;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text.Json;
using ExchangeType = RabbitMQ.Client.ExchangeType;

namespace Eventix.Shared.Infrastructure.EventBus
{
    internal sealed class EventBus : IEventBus
    {
        private readonly ILogger<EventBus> _logger;
        private const int RETRY_COUNT = 5;
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection = default!;
        private IChannel _channel = default!;
        private readonly string _brokerUrl;

        public EventBus(string brokerUrl, ILogger<EventBus> logger)
        {
            _brokerUrl = brokerUrl;
            _logger = logger;
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

            var exchangeName = typeof(T).FullName ?? string.Empty;

            await _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic, true, cancellationToken: cancellationToken);

            var body = JsonSerializer.SerializeToUtf8Bytes(integrationEvent);
            var properties = new BasicProperties
            {
                MessageId = integrationEvent.Id.ToString(),
                Persistent = true,
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            await _channel.BasicPublishAsync(exchangeName, string.Empty, false, properties, body, cancellationToken);

            _logger.LogInformation("Published integration event {EventName} with ID {EventId} to exchange {ExchangeName}.",
                typeof(T).Name, integrationEvent.Id, exchangeName);
        }

        public async Task SubscribeAsync<T>(
            string queueName,
            Func<T, Task> onMessage,
            CancellationToken cancellationToken = default
            ) where T : IntegrationEvent
        {
            await EnsureConnectedAsync();

            var exchangeName = typeof(T).FullName ?? string.Empty;

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

                _logger.LogInformation("Received integration event {EventName} with ID {EventId} from queue {QueueName}.",
                    typeof(T).Name, message?.Id, queueName);

                if (message is not null)
                    await onMessage(message);

                _logger.LogInformation("Processed integration event {EventName} with ID {EventId} from queue {QueueName}.",
                    typeof(T).Name, message?.Id, queueName);
            };

            await _channel.BasicConsumeAsync(queueName, true, consumer, cancellationToken: cancellationToken);
        }

        private async Task TryConnect()
        {
            var policy = Policy
                .Handle<BrokerUnreachableException>()
                .Or<IOException>()
                .Or<SocketException>()
                .WaitAndRetryAsync(RETRY_COUNT, retry =>
                {
                    _logger.LogWarning("Attempting to connect to RabbitMQ. Retry {RetryCount}/{MaxRetries}.", retry + 1, RETRY_COUNT);
                    return TimeSpan.FromSeconds(Math.Pow(2, retry));
                });

            await policy.ExecuteAsync(async () =>
            {
                _connection = await _connectionFactory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
                _logger.LogInformation("Successfully connected to RabbitMQ.");
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