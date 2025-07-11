using Eventix.Modules.Attendance.Application.Events.DTOs;
using Eventix.Modules.Attendance.Domain.Events.Models;

namespace Eventix.Modules.Attendance.Application.Events.UseCases.GetStatistics
{
    public sealed record GetEventStatisticsResponse(
    Guid EventId,
    string Title,
    string Description,
    LocationResponse? Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    int TicketsSold,
    int AttendeesCheckedIn,
    CheckInTicket[] DuplicateCheckInTickets,
    CheckInTicket[] InvalidCheckInTickets);
}