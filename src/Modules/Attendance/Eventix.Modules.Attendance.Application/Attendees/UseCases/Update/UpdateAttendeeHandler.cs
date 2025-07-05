using Eventix.Modules.Attendance.Domain.Attendees.Errors;
using Eventix.Modules.Attendance.Domain.Attendees.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Attendance.Application.Attendees.UseCases.Update
{
    internal sealed class UpdateAttendeeHandler(IAttendeeRepository attendeeRepository) : ICommandHandler<UpdateAttendeeCommand>
    {
        public async Task<Result> ExecuteAsync(UpdateAttendeeCommand request, CancellationToken cancellationToken = default)
        {
            var attendee = await attendeeRepository.GetByIdAsync(request.AttendeeId, cancellationToken);
            if (attendee is null)
                return Result.Failure(AttendeeErrors.NotFound(request.AttendeeId));

            attendee.UpdateName(request.FirstName, request.LastName);
            attendeeRepository.Update(attendee);

            var saveChanges = await attendeeRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges ? Result.Success() : Result.Failure(AttendeeErrors.SomethingHasFailedDuringPersistence);
        }
    }
}