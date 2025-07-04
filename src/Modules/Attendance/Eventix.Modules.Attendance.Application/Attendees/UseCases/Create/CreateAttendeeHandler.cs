using Eventix.Modules.Attendance.Domain.Attendees.Entities;
using Eventix.Modules.Attendance.Domain.Attendees.Errors;
using Eventix.Modules.Attendance.Domain.Attendees.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Attendance.Application.Attendees.UseCases.Create
{
    internal sealed class CreateAttendeeHandler(IAttendeeRepository attendeeRepository) : ICommandHandler<CreateAttendeeCommand>
    {
        public async Task<Result> ExecuteAsync(CreateAttendeeCommand request, CancellationToken cancellationToken = default)
        {
            var attendee = Attendee.Create(request.AttendeeId, request.Email, request.FirstName, request.LastName);

            attendeeRepository.Insert(attendee);

            var saveChanges = await attendeeRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges
                ? Result.Success()
                : Result.Failure(AttendeeErrors.SomethingHasFailedDuringPersistence);
        }
    }
}