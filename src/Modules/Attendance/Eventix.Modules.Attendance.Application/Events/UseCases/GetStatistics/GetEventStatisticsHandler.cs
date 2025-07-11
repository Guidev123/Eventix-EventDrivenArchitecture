using Dapper;
using Eventix.Modules.Attendance.Application.Events.DTOs;
using Eventix.Modules.Attendance.Application.Events.Mappers;
using Eventix.Modules.Attendance.Domain.Events.Errors;
using Eventix.Modules.Attendance.Domain.Events.Models;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;
using System.Data;

namespace Eventix.Modules.Attendance.Application.Events.UseCases.GetStatistics
{
    internal sealed class GetEventStatisticsHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetEventStatisticsQuery, GetEventStatisticsResponse>
    {
        public async Task<Result<GetEventStatisticsResponse>> ExecuteAsync(GetEventStatisticsQuery request, CancellationToken cancellationToken = default)
        {
            using var connectionEventStatistics = sqlConnectionFactory.Create();
            using var connectionDuplicatedTickets = sqlConnectionFactory.Create();
            using var connectionInvalidTickets = sqlConnectionFactory.Create();

            var eventStatisticsTask = GetEventStatisticsRawAsync(connectionEventStatistics, request.EventId, cancellationToken);
            var duplicatedTicketsTask = GetDuplicatedTicketsAsync(connectionDuplicatedTickets, request.EventId, cancellationToken);
            var invalidTicketsTask = GetInvalidTicketsAsync(connectionInvalidTickets, request.EventId, cancellationToken);

            await Task.WhenAll(eventStatisticsTask, duplicatedTicketsTask, invalidTicketsTask);

            if (eventStatisticsTask.Result.IsFailure && eventStatisticsTask.Result.Error is not null)
                return Result.Failure<GetEventStatisticsResponse>(eventStatisticsTask.Result.Error);

            var eventStatistics = eventStatisticsTask.Result.Value;
            var duplicatedTickets = duplicatedTicketsTask.Result;
            var invalidTickets = invalidTicketsTask.Result;

            var location = eventStatistics.MapToLocation();

            return Result.Success(eventStatistics.MapToGetEventStatisticsResponse(
                location,
                duplicatedTickets.ToArray(),
                invalidTickets.ToArray())
                );
        }

        private static async Task<Result<EventStatisticsRawResponse>> GetEventStatisticsRawAsync(IDbConnection connection, Guid eventId, CancellationToken cancellationToken)
        {
            const string sql = $@"
            SELECT
                EventId,
                Title,
                Description,
                Street,
                Number,
                AdditionalInfo,
                Neighborhood,
                ZipCode,
                City,
                State,
                StartsAtUtc,
                EndsAtUtc,
                TicketsSold,
                AttendeesCheckedIn
            FROM attendance.EventStatistics
            WHERE EventId = @EventId";

            var eventStatistics = await connection.QuerySingleOrDefaultAsync<EventStatisticsRawResponse>(sql, new { EventId = eventId });

            return eventStatistics is not null
             ? Result.Success(eventStatistics)
             : Result.Failure<EventStatisticsRawResponse>(EventErrors.StatisticsNotFound(eventId));
        }

        private static async Task<List<InvalidCheckInTicket>> GetInvalidTicketsAsync(IDbConnection connection, Guid eventId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT EventId, Code, Count
                FROM attendance.InvalidCheckInTickets
                WHERE EventId = @EventId";

            return (await connection.QueryAsync<CheckInTicketRawResponse>(sql, new { EventId = eventId }))
                .Select(t => new InvalidCheckInTicket(t.Code, t.EventId, t.Count))
                .ToList();
        }

        private static async Task<List<DuplicateCheckInTicket>> GetDuplicatedTicketsAsync(IDbConnection connection, Guid eventId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT EventId, Code, Count
                FROM attendance.DuplicateCheckInTickets
                WHERE EventId = @EventId";

            return (await connection.QueryAsync<CheckInTicketRawResponse>(sql, new { EventId = eventId }))
                .Select(t => new DuplicateCheckInTicket(t.Code, t.EventId, t.Count))
                .ToList();
        }
    }
}