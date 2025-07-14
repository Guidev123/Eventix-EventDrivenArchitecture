using Dapper;
using Eventix.Modules.Attendance.Domain.Attendees.DomainEvents;
using Eventix.Modules.Attendance.Domain.Events.Models;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;
using System.Data;

namespace Eventix.Modules.Attendance.Application.Attendees.DomainEvents
{
    internal sealed class InvalidCheckInAttemptedDomainEventHandler(ISqlConnectionFactory sqlConnectionFactory) : DomainEventHandler<InvalidCheckInAttemptDomainEvent>
    {
        public override async Task ExecuteAsync(InvalidCheckInAttemptDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();

            var invalidCheckInAttempt = await GetInvalidCheckInAttemptAsync(domainEvent.EventId, domainEvent.TicketCode, connection);

            if (invalidCheckInAttempt is not null)
                await IncrementCountAsync(invalidCheckInAttempt, connection);
            else
                await CreateCheckInTicketsAsync(domainEvent, connection);
        }

        private static async Task IncrementCountAsync(InvalidCheckInTicket invalidCheckInTicket, IDbConnection connection)
        {
            invalidCheckInTicket.IncrementCount();

            const string sql = """
                UPDATE attendance.InvalidCheckInTickets
                SET Count = @Count
                WHERE EventId = @EventId AND @Code = code
                """;

            await connection.ExecuteAsync(sql, new { invalidCheckInTicket.EventId, invalidCheckInTicket.Code, invalidCheckInTicket.Count });
        }

        private static async Task CreateCheckInTicketsAsync(InvalidCheckInAttemptDomainEvent domainEvent, IDbConnection connection)
        {
            const string sql = @"
                INSERT INTO attendance.InvalidCheckInTickets (EventId, Code, Count)
                VALUES (@EventId, @Code, @Count)";

            await connection.ExecuteAsync(sql, new
            {
                domainEvent.EventId,
                Code = domainEvent.TicketCode,
                Count = (int)decimal.One
            });
        }

        private static async Task<InvalidCheckInTicket?> GetInvalidCheckInAttemptAsync(
            Guid eventId,
            string code,
            IDbConnection dbConnection
            )
        {
            const string sql = """
                SELECT
                    EventId,
                    Code,
                    Count
                FROM attendance.InvalidCheckInTickets
                WHERE EventId = @EventId AND @Code = code
                """;

            return await dbConnection.QueryFirstOrDefaultAsync<InvalidCheckInTicket>(sql, new { EventId = eventId, Code = code });
        }
    }
}