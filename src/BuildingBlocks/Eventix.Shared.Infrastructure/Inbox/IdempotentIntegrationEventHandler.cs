using Dapper;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Infrastructure.Inbox.Models;
using System.Data.Common;

namespace Eventix.Shared.Infrastructure.Inbox
{
    public abstract class IdempotentIntegrationEventHandler<TIntegrationEvent> : IntegrationEventHandler<TIntegrationEvent>
        where TIntegrationEvent : IIntegrationEvent
    {
        protected static async Task<bool> IsInboxMessageProcessedAsync(
           InboxMessageConsumer inboxMessageConsumer,
           DbConnection connection,
           string schema)
        {
            var sql = $@"
                SELECT EXISTS(
                    SELECT 1
                    FROM {schema}.InboxMessages
                    WHERE OutboxMessageId = @InboxMessageId
                      AND Name = @Name
                )";

            return await connection.ExecuteScalarAsync<bool>(sql, inboxMessageConsumer);
        }

        protected static async Task MarkInboxMessageAsProcessedAsync(
            InboxMessageConsumer inboxMessageConsumer,
            DbConnection connection,
            string schema)
        {
            var sql = $@"
                INSERT INTO {schema}.InboxMessages (InboxMessageId, Name)
                VALUES (@InboxMessageId, @Name)";

            await connection.ExecuteAsync(sql, inboxMessageConsumer);
        }
    }
}