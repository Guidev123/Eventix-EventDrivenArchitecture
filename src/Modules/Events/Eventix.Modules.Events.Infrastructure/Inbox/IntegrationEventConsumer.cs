using Dapper;
using Eventix.Modules.Events.Infrastructure.Database;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Infrastructure.Extensions;
using Eventix.Shared.Infrastructure.Inbox.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Eventix.Modules.Events.Infrastructure.Inbox
{
    internal sealed class IntegrationEventConsumer(
        IEventBus eventBus,
        ISqlConnectionFactory sqlConnectionFactory
        ) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers(stoppingToken);

            return Task.CompletedTask;
        }

        private async Task ConsumeAsync(IIntegrationEvent integrationEvent)
        {
            using var connection = sqlConnectionFactory.Create();

            var inboxMessage = new InboxMessage
            {
                Id = integrationEvent.CorrelationId,
                Content = JsonConvert.SerializeObject(integrationEvent, SerializerExtension.Instance),
                Type = integrationEvent.GetType().Name,
                OccurredOnUtc = integrationEvent.OccurredOnUtc
            };

            var sql = $@"
                INSERT INTO {Schemas.Events}.InboxMessages (Id, Type, Content, OccurredOnUtc)
                VALUES (@Id, @Type, @Content, @OccurredOnUtc)";

            await connection.ExecuteAsync(sql, inboxMessage);
        }

        private void SetSubscribers(CancellationToken cancellationToken = default)
        {
        }
    }
}