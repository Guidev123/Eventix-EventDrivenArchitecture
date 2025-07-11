using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Attendance.Application.Attendees.UseCases.CheckIn
{
    public sealed record CheckInAttendeeCommand(
        Guid AttendeeId,
        Guid TicketId
        ) : ICommand;
}