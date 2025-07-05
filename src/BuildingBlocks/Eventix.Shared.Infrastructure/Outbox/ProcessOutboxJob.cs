using Dapper;
using Eventix.Shared.Application.Clock;
using Eventix.Shared.Application.Exceptions;
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
                Error = GetExceptionMessage(exception)
            }, transaction: transaction);
        }

        private static string GetExceptionMessage(Exception? exception)
        {
            return exception switch
            {
                EventixException eventixException when eventixException.Error?.Description is not null => eventixException.Error.Description,
                _ when exception?.InnerException?.Message is not null => exception.InnerException.Message,
                _ => exception?.Message ?? "Unknown error"
            };
        }
    }
}