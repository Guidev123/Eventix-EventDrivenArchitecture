using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Attendance.Application.Attendees.UseCases.GetById
{
    public sealed record GetAttendeeByIdQuery(Guid CustomerId) : IQuery<GetAttendeeByIdResponse>;
}