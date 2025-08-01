using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text.Json;

namespace Eventix.Shared.Infrastructure.EventBus
{
    internal sealed class EventBus : IEventBus
    {
        private readonly BrokerOptions _brokerOptions = new();
        private readonly BusResilienceConfiguration _busResilienceOptions = new();

        private readonly ILogger<EventBus> _logger;
        private readonly IBusFailureHandlingService _busFailureHandlingService;

        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection = default!;
        private IChannel _channel = default!;

        public EventBus(
            Action<BrokerOptions> configureBroker,
            ILogger<EventBus> logger,
            IBusFailureHandlingService busFailureHandlingService,
            Action<BusResilienceConfiguration>? configureResilience = null)
        {
            configureBroker(_brokerOptions);
            if (configureResilience is not null)
            {
                configureResilience(_busResilienceOptions);
            }

            _connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(_brokerOptions.ConnectionString),
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(_brokerOptions.NetworkRecoveryIntervalInSeconds)
            };

            _logger = logger;
            _busFailureHandlingService = busFailureHandlingService;
        }

        public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
             where T : IntegrationEvent
        {
            await EnsureConnectedAsync();

            var exchangeName = string.Empty.GetExchangeName<T>();
            var routingKey = string.Empty.GetRoutingKey<T>();

            await _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic, true, cancellationToken: cancellationToken);

            var body = JsonSerializer.SerializeToUtf8Bytes(integrationEvent);
            var properties = new BasicProperties
            {
                MessageId = integrationEvent.CorrelationId.ToString(),
                Persistent = true,
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            await _channel.BasicPublishAsync(exchangeName, routingKey, false, properties, body, cancellationToken);

            _logger.LogInformation("Published integration event {EventName} with ID {CorrelationId} to exchange {ExchangeName}.",
                typeof(T).Name, integrationEvent.CorrelationId, exchangeName);
        }

        public async Task SubscribeAsync<T>(
            string queueName,
            Func<T, Task> onMessage,
            CancellationToken cancellationToken = default
            ) where T : IntegrationEvent
        {
            await EnsureConnectedAsync();
            await _busFailureHandlingService.DeclareRetryInfrastructureAsync<T>(queueName, _channel, cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, eventArgs) =>
            {
                var deliveryTag = eventArgs.DeliveryTag;
                var correlationId = eventArgs.BasicProperties?.MessageId ?? string.Empty;

                try
                {
                    var body = eventArgs.Body.ToArray();
                    var message = JsonSerializer.Deserialize<T>(body);

                    if (message is null)
                    {
                        _logger.LogError("Failed to deserialize message {CorrelationId} from queue {QueueName}.",
                            correlationId, queueName);
                        await _channel.BasicAckAsync(deliveryTag, false);
                        return;
                    }

                    _logger.LogInformation("Received integration event {EventName} with ID {CorrelationId} from queue {QueueName}.",
                        typeof(T).Name, message.CorrelationId, queueName);

                    await onMessage(message);

                    await _channel.BasicAckAsync(deliveryTag, false);

                    _logger.LogInformation("Processed integration event {EventName} with ID {CorrelationId} from queue {QueueName}.",
                        typeof(T).Name, message.CorrelationId, queueName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message {CorrelationId} from queue {QueueName}.",
                        correlationId, queueName);

                    var retryCount = _busFailureHandlingService.GetRetryCount(eventArgs.BasicProperties);

                    if (retryCount < _busResilienceOptions.MaxDeliveryRetryAttempts)
                    {
                        await _busFailureHandlingService.SendToRetryQueueAsync(
                            eventArgs,
                            queueName,
                            retryCount + 1,
                            correlationId,
                            _busResilienceOptions,
                            _channel, cancellationToken);

                        await _channel.BasicAckAsync(deliveryTag, false);

                        _logger.LogWarning("Message {CorrelationId} sent to retry queue. Attempt {RetryCount}/{MaxDeliveryRetryAttempts}.",
                            correlationId, retryCount + 1, _busResilienceOptions.MaxDeliveryRetryAttempts);
                    }
                    else
                    {
                        _logger.LogError("Message {CorrelationId} from queue {QueueName} exceeded maximum retries ({MaxDeliveryRetryAttempts}). Message will be acknowledged and discarded.",
                            correlationId, queueName, _busResilienceOptions.MaxDeliveryRetryAttempts);

                        await _channel.BasicAckAsync(deliveryTag, false);
                    }
                }
            };

            await _channel.BasicConsumeAsync(queueName, false, consumer, cancellationToken: cancellationToken);
        }

        private async Task TryConnect()
        {
            var policy = Policy
                .Handle<BrokerUnreachableException>()
                .Or<IOException>()
                .Or<SocketException>()
                .WaitAndRetryAsync(_brokerOptions.TryConnectMaxRetries, retry =>
                {
                    _logger.LogWarning("Attempting to connect to RabbitMQ. Retry {RetryCount}/{TryConnectMaxRetries}.",
                        retry, _brokerOptions.TryConnectMaxRetries);
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