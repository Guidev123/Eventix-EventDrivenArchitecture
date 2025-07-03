using Dapper;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.DomainEvents;
using Eventix.Shared.Infrastructure.Outbox.Models;
using System.Data.Common;

namespace Eventix.Shared.Infrastructure.Outbox
{
    public abstract class IdempotentDomainEventHandler<TDomainEvent> : DomainEventHandler<TDomainEvent> where TDomainEvent : IDomainEvent
    {
        protected static async Task<bool> IsOutboxMessageProcessedAsync(
           OutboxMessageConsumer outboxMessageConsumer,
           DbConnection connection,
           string schema)
        {
            var sql = $@"
                SELECT EXISTS(
                    SELECT 1
                    FROM {schema}.OutboxMessages
                    WHERE OutboxMessageId = @OutboxMessageId
                      AND Name = @Name
                )";

            return await connection.ExecuteScalarAsync<bool>(sql, outboxMessageConsumer);
        }

        protected static async Task MarkOutboxMessageAsProcessedAsync(
            OutboxMessageConsumer outboxMessageConsumer,
            DbConnection connection,
            string schema)
        {
            var sql = $@"
                INSERT INTO {schema}.OutboxMessages (OutboxMessageId, Name)
                VALUES (@OutboxMessageId, @Name)";

            await connection.ExecuteAsync(sql, outboxMessageConsumer);
        }
    }
}