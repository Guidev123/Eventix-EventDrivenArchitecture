using Dapper;
using Eventix.Shared.Application.Clock;
using Eventix.Shared.Application.Exceptions;
using Eventix.Shared.Infrastructure.Inbox.Models;
using System.Data;

namespace Eventix.Shared.Infrastructure.Inbox
{
    public abstract class ProcessInboxJob(IDateTimeProvider dateTimeProvider)
    {
        protected static async Task<IReadOnlyList<InboxMessageResponse>> GetInboxMessagesAsync(
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
                FROM {schema}.InboxMessages WITH (UPDLOCK, ROWLOCK)
                WHERE ProcessedOnUtc IS NULL
                ORDER BY OccurredOnUtc";

            var inboxMessages = await connection.QueryAsync<InboxMessageResponse>(sql, transaction: transaction);

            return inboxMessages.ToList();
        }

        protected async Task UpdateInboxMessageAsync(
            IDbConnection connection,
            IDbTransaction transaction,
            InboxMessageResponse inboxMessage,
            Exception? exception,
            string schema
            )
        {
            var sql = @$"
                UPDATE {schema}.InboxMessages
                SET ProcessedOnUtc = @ProcessedOnUtc,
                    Error = @Error
                WHERE Id = @Id";

            await connection.ExecuteAsync(sql, new
            {
                inboxMessage.Id,
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