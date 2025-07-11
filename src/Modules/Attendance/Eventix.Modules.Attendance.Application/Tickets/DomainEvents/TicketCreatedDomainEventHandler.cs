using Dapper;
using Eventix.Modules.Attendance.Domain.Tickets.DomainEvents;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Attendance.Application.Tickets.DomainEvents
{
    internal sealed class TicketCreatedDomainEventHandler(ISqlConnectionFactory sqlConnectionFactory) : DomainEventHandler<TicketCreatedDomainEvent>
    {
        public override async Task ExecuteAsync(TicketCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();

            const string sql = $@"
                UPDATE attendance.EventStatistics
                SET TicketsSold = (
                    SELECT COUNT(*)
                    FROM attendance.Tickets
                    WHERE Tickets.EventId = EventStatistics.EventId)
                WHERE EventStatistics.EventId = @EventId";

            await connection.ExecuteAsync(sql, new { domainEvent.EventId });
        }
    }
}