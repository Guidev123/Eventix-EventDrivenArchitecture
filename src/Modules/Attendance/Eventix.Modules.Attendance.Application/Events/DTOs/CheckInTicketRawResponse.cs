namespace Eventix.Modules.Attendance.Application.Events.DTOs
{
    public sealed record CheckInTicketRawResponse(
        Guid EventId,
        string Code,
        int Count
        );
}