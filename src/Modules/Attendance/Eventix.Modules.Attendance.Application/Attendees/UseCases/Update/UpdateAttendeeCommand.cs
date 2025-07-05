using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Attendance.Application.Attendees.UseCases.Update
{
    public sealed record UpdateAttendeeCommand : ICommand
    {
        public UpdateAttendeeCommand(
        string firstName,
        string lastName
        )
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public Guid AttendeeId { get; private set; }
        public string FirstName { get; }
        public string LastName { get; }
        public void SetAttendeeId(Guid attendeeId) => AttendeeId = attendeeId;
    }
}