using Dapper;
using Eventix.Shared.Application.Clock;
using Eventix.Shared.Infrastructure.Outbox.Models;
using System.Data;

namespace Eventix.Shared.Infrastructure.Outbox
{
    public abstract class ProcessOutboxJob(IDateTimeProvider dateTimeProvider)
    {
        protected static async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(
            IDbConnection connection,
            IDbTransaction transaction,
            int batchSize,
            string schema
            )
        {
            var sql = @$"
                SELECT TOP ({batchSize})
                    Id,
                    Content
                FROM {schema}.OutboxMessages WITH (UPDLOCK, ROWLOCK)
                WHERE ProcessedOnUtc IS NULL
                ORDER BY OccurredOnUtc";

            var outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(sql, transaction: transaction);

            return outboxMessages.ToList();
        }

        protected async Task UpdateOutboxMessageAsync(
            IDbConnection connection,
            IDbTransaction transaction,
            OutboxMessageResponse outboxMessage,
            Exception? exception,
            string schema
            )
        {
            var sql = @$"
                UPDATE {schema}.OutboxMessages
                SET ProcessedOnUtc = @ProcessedOnUtc,
                    Error = @Error
                WHERE Id = @Id";

            await connection.ExecuteAsync(sql, new
            {
                outboxMessage.Id,
                ProcessedOnUtc = dateTimeProvider.UtcNow,
                Error = exception?.ToString()
            }, transaction: transaction);
        }
    }
}