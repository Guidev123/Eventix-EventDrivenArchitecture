using Dapper;
using Eventix.Modules.Attendance.Domain.Attendees.DomainEvents;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Attendance.Application.Attendees.DomainEvents
{
    internal sealed class AttendeeCheckedInDomainEventHandler(ISqlConnectionFactory sqlConnectionFactory)
        : DomainEventHandler<AttendeeCheckedInDomainEvent>
    {
        public override async Task ExecuteAsync(AttendeeCheckedInDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();

            const string sql = @"
                UPDATE attendance.EventStatistics
                SET AttendeesCheckedIn = (
                    SELECT COUNT(*)
                    FROM attendance.Tickets
                    WHERE
                        Tickets.EventId = EventStatistics.EventId AND
                        Tickets.UsedAtUtc IS NOT NULL
                )
                WHERE EventStatistics.EventId = @EventId";

            await connection.ExecuteAsync(
                sql,
                new { domainEvent.EventId })
                .ConfigureAwait(false);
        }
    }
}