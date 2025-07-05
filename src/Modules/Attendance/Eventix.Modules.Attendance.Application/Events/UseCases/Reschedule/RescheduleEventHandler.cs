using Eventix.Modules.Attendance.Domain.Events.Errors;
using Eventix.Modules.Attendance.Domain.Events.Interfaces;
using Eventix.Shared.Application.Clock;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Attendance.Application.Events.UseCases.Reschedule
{
    internal sealed class RescheduleEventHandler(
        IDateTimeProvider dateTimeProvider,
        IEventRepository eventRepository) : ICommandHandler<RescheduleEventCommand>
    {
        public async Task<Result> ExecuteAsync(RescheduleEventCommand request, CancellationToken cancellationToken = default)
        {
            var @event = await eventRepository.GetByIdAsync(request.EventId, cancellationToken);
            if (@event is null)
                return Result.Failure(EventErrors.NotFound(request.EventId));

            if (request.StartsAtUtc < dateTimeProvider.UtcNow)
                return Result.Failure(EventErrors.StartDateInPast);

            @event.Reschedule(request.StartsAtUtc, request.EndsAtUtc);
            eventRepository.Update(@event);

            var saveChanges = await eventRepository.UnitOfWork.CommitAsync(cancellationToken);
            return saveChanges
                ? Result.Success()
                : Result.Failure(EventErrors.FailToRescheduleEvent);
        }
    }
}