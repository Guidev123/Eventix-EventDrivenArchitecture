using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Attendance.Application.Events.UseCases.GetStatistics
{
    public sealed record GetEventStatisticsQuery(Guid EventId) : IQuery<GetEventStatisticsResponse>;
}