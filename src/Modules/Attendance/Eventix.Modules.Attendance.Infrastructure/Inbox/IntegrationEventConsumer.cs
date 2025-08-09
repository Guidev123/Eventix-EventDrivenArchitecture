using Dapper;
using Eventix.Modules.Attendance.Infrastructure.Database;
using Eventix.Modules.Events.IntegrationEvents.Events;
using Eventix.Modules.Ticketing.IntegrationEvents.Tickets;
using Eventix.Modules.Users.IntegrationEvents.Users;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Infrastructure.Extensions;
using Eventix.Shared.Infrastructure.Inbox.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Data;

namespace Eventix.Modules.Attendance.Infrastructure.Inbox
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
            eventBus.SubscribeAsync<UserRegisteredIntegrationEvent>("attendance-user-registered", ConsumeAsync, ExchangeTypeEnum.Direct, cancellationToken);
            eventBus.SubscribeAsync<UserUpdatedIntegrationEvent>("attendance-user-updated", ConsumeAsync, cancellationToken);

            eventBus.SubscribeAsync<EventRescheduledIntegrationEvent>("attendance-event-rescheduled", ConsumeAsync, cancellationToken);
            eventBus.SubscribeAsync<EventPublishedIntegrationEvent>("attendance-event-published", ConsumeAsync, cancellationToken);
            eventBus.SubscribeAsync<EventCancelledIntegrationEvent>("attendance-event-cancelled", ConsumeAsync, cancellationToken);

            eventBus.SubscribeAsync<TicketCreatedIntegrationEvent>("attendance-ticket-created", ConsumeAsync, cancellationToken);
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
                INSERT INTO {Schemas.Attendance}.InboxMessages (Id, Type, Content, OccurredOnUtc)
                VALUES (@Id, @Type, @Content, @OccurredOnUtc)";

            await connection.ExecuteAsync(sql, inboxMessage);
        }

        private static async Task<bool> MessageAlreadyExistsAsync(IDbConnection connection, Guid CorrelationId)
        {
            var sql = $""""
                SELECT CASE WHEN EXISTS(
                        SELECT 1
                        FROM {Schemas.Attendance}.InboxMessages
                        WHERE Id = @CorrelationId)
                    THEN 1
                    ELSE 0
                END
                """";

            return await connection.ExecuteScalarAsync<bool>(sql, new { CorrelationId });
        }
    }
}