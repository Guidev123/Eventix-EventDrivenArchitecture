using Eventix.Shared.Application.EventBus;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Eventix.Shared.Infrastructure.EventBus
{
    internal interface IBusFailureHandlingService
    {
        Task DeclareRetryInfrastructureAsync<T>(string queueName, IChannel channel, CancellationToken cancellationToken = default)
            where T : IntegrationEvent;

        int GetRetryCount(IReadOnlyBasicProperties? properties);

        Task SendToRetryQueueAsync(
            BasicDeliverEventArgs eventArgs,
            string queueName,
            int retryCount,
            string correlationId,
            BusResilienceConfiguration busResilience,
            IChannel channel,
            CancellationToken cancellationToken = default);
    }
}