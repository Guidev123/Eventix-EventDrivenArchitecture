using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Attendance.Application.Attendees.UseCases.Create
{
    public sealed record CreateAttendeeCommand(
        Guid AttendeeId,
        string Email,
        string FirstName,
        string LastName
        ) : ICommand;
}