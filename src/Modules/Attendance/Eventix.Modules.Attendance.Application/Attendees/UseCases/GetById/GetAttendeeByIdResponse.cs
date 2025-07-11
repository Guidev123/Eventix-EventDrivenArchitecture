namespace Eventix.Modules.Attendance.Application.Attendees.UseCases.GetById
{
    public sealed record GetAttendeeByIdResponse(
        Guid AttendeeId,
        string Email,
        string FirstName,
        string LastName
        );
}