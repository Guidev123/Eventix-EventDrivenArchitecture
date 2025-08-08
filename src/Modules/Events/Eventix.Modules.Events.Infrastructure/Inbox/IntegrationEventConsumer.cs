using Dapper;
using Eventix.Modules.Events.Infrastructure.Database;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Infrastructure.Extensions;
using Eventix.Shared.Infrastructure.Inbox.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Data;

namespace Eventix.Modules.Events.Infrastructure.Inbox
{
    internal sealed class IntegrationEventConsumer(
        IBus eventBus,
        ISqlConnectionFactory sqlConnectionFactory
        ) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers(stoppingToken);

            return Task.CompletedTask;
        }

        private void SetSubscribers(CancellationToken cancellationToken = default)
        {
        }

        private async Task ConsumeAsync(IIntegrationEvent integrationEvent)
        {
            using var connection = sqlConnectionFactory.Create();

            if (await MessageAlreadyExistsAsync(connection, integrationEvent.CorrelationId))
            {
                return;
            }

            var inboxMessage = new InboxMessage
            {
                Id = integrationEvent.CorrelationId,
                Content = JsonConvert.SerializeObject(integrationEvent, SerializerExtensions.Instance),
                Type = integrationEvent.GetType().Name,
                OccurredOnUtc = integrationEvent.OccurredOnUtc
            };

            var sql = $@"
                INSERT INTO {Schemas.Events}.InboxMessages (Id, Type, Content, OccurredOnUtc)
                VALUES (@Id, @Type, @Content, @OccurredOnUtc)";

            await connection.ExecuteAsync(sql, inboxMessage);
        }

        private static async Task<bool> MessageAlreadyExistsAsync(IDbConnection connection, Guid CorrelationId)
        {
            var sql = $""""
                SELECT CASE WHEN EXISTS(
                        SELECT 1
                        FROM {Schemas.Events}.InboxMessages
                        WHERE Id = @CorrelationId)
                    THEN 1
                    ELSE 0
                END
                """";

            return await connection.ExecuteScalarAsync<bool>(sql, new { CorrelationId });
        }
    }
}