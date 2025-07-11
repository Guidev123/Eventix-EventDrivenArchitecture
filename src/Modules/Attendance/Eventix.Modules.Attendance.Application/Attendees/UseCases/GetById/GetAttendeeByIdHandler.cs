using Eventix.Modules.Attendance.Domain.Attendees.Errors;
using Eventix.Modules.Attendance.Domain.Attendees.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Attendance.Application.Attendees.UseCases.GetById
{
    internal sealed class GetAttendeeByIdHandler(IAttendeeRepository attendeeRepository) : IQueryHandler<GetAttendeeByIdQuery, GetAttendeeByIdResponse>
    {
        public async Task<Result<GetAttendeeByIdResponse>> ExecuteAsync(GetAttendeeByIdQuery request, CancellationToken cancellationToken = default)
        {
            var attendee = await attendeeRepository.GetByIdAsync(request.CustomerId, cancellationToken);
            if (attendee is null)
                return Result.Failure<GetAttendeeByIdResponse>(AttendeeErrors.NotFound(request.CustomerId));

            return Result.Success(new GetAttendeeByIdResponse(
                attendee.Id,
                attendee.Email.Address,
                attendee.Name.FirstName,
                attendee.Name.LastName
                ));
        }
    }
}