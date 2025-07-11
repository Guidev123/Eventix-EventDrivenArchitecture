namespace Eventix.Modules.Attendance.Application.Events.DTOs
{
    public sealed record EventStatisticsRawResponse(
        Guid EventId,
        string Title,
        string Description,
        string Street,
        string Number,
        string AdditionalInfo,
        string Neighborhood,
        string ZipCode,
        string City,
        string State,
        DateTime StartsAtUtc,
        DateTime? EndsAtUtc,
        int TicketsSold,
        int AttendeesCheckedIn
        );
}