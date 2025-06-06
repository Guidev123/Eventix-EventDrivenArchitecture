using Eventix.Modules.Events.Application.Abstractions.Clock;
using Eventix.Modules.Events.Application.Abstractions.Data;
using Eventix.Modules.Events.Application.Abstractions.Messaging;
using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.Domain.Events.Interfaces;
using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Application.Events.Reschedule
{
    public sealed class RescheduleEventHandler(IEventRepository eventRepository,
                                               IUnitOfWork unitOfWork,
                                               IDateTimeProvider dateTimeProvider) : ICommandHandler<RescheduleEventCommand>
    {
        public async Task<Result> ExecuteAsync(RescheduleEventCommand request, CancellationToken cancellationToken = default)
        {
            var @event = await eventRepository.GetByIdAsync(request.EventId, cancellationToken);

            if (@event is null)
                return Result.Failure(EventErrors.NotFound(request.EventId));

            if (request.StartsAtUtc < dateTimeProvider.UtcNow)
                return Result.Failure(EventErrors.StartDateInPast);

            @event.Reschedule(request.StartsAtUtc, request.EndsAtUtc);

            var rows = await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return rows > 0
                ? Result.Success()
                : Result.Failure(EventErrors.UnableToCancelEvent(request.EventId));
        }
    }
}