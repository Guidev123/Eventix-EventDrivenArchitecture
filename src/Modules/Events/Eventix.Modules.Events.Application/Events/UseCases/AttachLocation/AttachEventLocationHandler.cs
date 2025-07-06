using Eventix.Modules.Events.Application.Events.Mappers;
using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.Domain.Events.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.Events.UseCases.AttachLocation
{
    internal sealed class AttachEventLocationHandler(IEventRepository eventRepository) : ICommandHandler<AttachEventLocationCommand>
    {
        public async Task<Result> ExecuteAsync(AttachEventLocationCommand request, CancellationToken cancellationToken = default)
        {
            var @event = await eventRepository.GetByIdAsync(request.EventId, cancellationToken).ConfigureAwait(false);
            if (@event is null)
                return Result.Failure(EventErrors.NotFound(request.EventId));

            @event.AttachLocation(request.MapToLocation());

            eventRepository.Update(@event);

            var saveChanges = await eventRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges
                ? Result.Success()
                : Result.Failure(EventErrors.UnableToAttachLocation);
        }
    }
}