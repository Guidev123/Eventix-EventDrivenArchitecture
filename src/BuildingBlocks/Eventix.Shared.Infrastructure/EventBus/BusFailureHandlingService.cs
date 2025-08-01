using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Eventix.Shared.Infrastructure.EventBus
{
    internal sealed class BusFailureHandlingService(ILogger<BusFailureHandlingService> logger) : IBusFailureHandlingService
    {
        public async Task DeclareRetryInfrastructureAsync<T>(string queueName, IChannel channel, CancellationToken cancellationToken = default)
             where T : IntegrationEvent
        {
            var exchangeName = string.Empty.GetExchangeName<T>();
            var routingKey = string.Empty.GetRoutingKey<T>();
            var retryQueueName = $"{queueName}.retry";

            await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic, true, cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(queueName, true, false, false, cancellationToken: cancellationToken);

            var retryQueueArgs = new Dictionary<string, object>
            {
                {"x-dead-letter-exchange", exchangeName},
                {"x-dead-letter-routing-key", routingKey}
            };

            await channel.QueueDeclareAsync(retryQueueName, true, false, false, retryQueueArgs!, cancellationToken: cancellationToken);

            await channel.QueueBindAsync(queueName, exchangeName, routingKey, cancellationToken: cancellationToken);
        }

        public async Task SendToRetryQueueAsync(
            BasicDeliverEventArgs eventArgs,
            string queueName,
            int retryCount,
            string correlationId,
            BusResilienceConfiguration busResilience,
            IChannel channel,
            CancellationToken cancellationToken)
        {
            var retryQueueName = $"{queueName}.retry";

            var baseDelayMs = busResilience.InitialDeliveryRetryDelay.TotalMilliseconds;
            var exponentialDelayMs = baseDelayMs * Math.Pow(2, retryCount - 1);

            var maxDelayMs = busResilience.MaxDeliveryRetryDelay.TotalMilliseconds;
            var delayMs = (int)Math.Min(exponentialDelayMs, maxDelayMs);

            var headers = new Dictionary<string, object>(eventArgs.BasicProperties.Headers!);

            var retryProperties = new BasicProperties
            {
                MessageId = eventArgs.BasicProperties.MessageId,
                Persistent = true,
                Timestamp = eventArgs.BasicProperties.Timestamp,
                Expiration = delayMs.ToString(),
                Headers = headers!
            };

            retryProperties.Headers["x-retry-count"] = retryCount;

            await channel.BasicPublishAsync("", retryQueueName, false, retryProperties, eventArgs.Body, cancellationToken);

            logger.LogInformation("Message {CorrelationId} sent to retry queue {RetryQueueName} with delay {DelayMs}ms (max: {MaxDelayMs}ms). Retry count: {RetryCount}",
                correlationId, retryQueueName, delayMs, maxDelayMs, retryCount);
        }

        public int GetRetryCount(IReadOnlyBasicProperties? properties)
        {
            if (properties?.Headers?.TryGetValue("x-retry-count", out var retryCountObj) == true)
            {
                return retryCountObj switch
                {
                    int count => count,
                    byte[] bytes => BitConverter.ToInt32(bytes, 0),
                    _ => 0
                };
            }
            return 0;
        }
    }
}