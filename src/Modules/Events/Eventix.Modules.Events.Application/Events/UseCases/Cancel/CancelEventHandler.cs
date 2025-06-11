using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.Domain.Events.Interfaces;
using Eventix.Shared.Application.Clock;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.Events.UseCases.Cancel
{
    internal sealed class CancelEventHandler(IEventRepository eventRepository,
                                             IDateTimeProvider dateTimeProvider,
                                             IUnitOfWork unitOfWork) : ICommandHandler<CancelEventCommand>
    {
        public async Task<Result> ExecuteAsync(CancelEventCommand request, CancellationToken cancellationToken = default)
        {
            var @event = await eventRepository.GetByIdAsync(request.EventId, cancellationToken).ConfigureAwait(false);
            if (@event is null)
                return Result.Failure(EventErrors.NotFound(request.EventId));

            var result = @event.Cancel(dateTimeProvider.UtcNow);
            if (result.IsFailure && result.Error is not null)
                return Result.Failure(result.Error);

            eventRepository.Update(@event);

            var saveChanges = await unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges ? Result.Success() : Result.Failure(EventErrors.UnableToCancelEvent(request.EventId));
        }
    }
}