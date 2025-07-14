using Dapper;
using Eventix.Modules.Attendance.Domain.Attendees.DomainEvents;
using Eventix.Modules.Attendance.Domain.Events.Models;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;
using System.Data;

namespace Eventix.Modules.Attendance.Application.Attendees.DomainEvents
{
    internal sealed class DuplicateCheckInAttemptedDomainEventHandler(ISqlConnectionFactory sqlConnectionFactory)
        : DomainEventHandler<DuplicateCheckInAttemptDomainEvent>
    {
        public override async Task ExecuteAsync(DuplicateCheckInAttemptDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();

            var duplicateCheckInTickets = await GetDuplicateCheckInTicketsAsync(domainEvent.EventId, domainEvent.TicketCode, connection);

            if (duplicateCheckInTickets is not null)
                await IncrementCountAsync(duplicateCheckInTickets, connection);
            else
                await CreateCheckInTicketsAsync(domainEvent, connection);
        }

        private static async Task IncrementCountAsync(DuplicateCheckInTicket duplicateCheckInTicket, IDbConnection connection)
        {
            duplicateCheckInTicket.IncrementCount();

            const string sql = """
                UPDATE attendance.DuplicateCheckInTickets
                SET Count = @Count
                WHERE EventId = @EventId AND @Code = code
                """;

            await connection.ExecuteAsync(sql, new { duplicateCheckInTicket.EventId, duplicateCheckInTicket.Code, duplicateCheckInTicket.Count });
        }

        private static async Task CreateCheckInTicketsAsync(DuplicateCheckInAttemptDomainEvent domainEvent, IDbConnection connection)
        {
            const string sql = @"
                INSERT INTO attendance.DuplicateCheckInTickets (EventId, Code, Count)
                VALUES (@EventId, @Code, @Count)";

            await connection.ExecuteAsync(sql, new
            {
                domainEvent.EventId,
                Code = domainEvent.TicketCode,
                Count = (int)decimal.One
            });
        }

        private static async Task<DuplicateCheckInTicket?> GetDuplicateCheckInTicketsAsync(
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
                FROM attendance.DuplicateCheckInTickets
                WHERE EventId = @EventId AND @Code = code
                """;

            return await dbConnection.QueryFirstOrDefaultAsync<DuplicateCheckInTicket>(sql, new { EventId = eventId, Code = code });
        }
    }
}