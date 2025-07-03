using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.Domain.Events.Interfaces;
using Eventix.Modules.Events.Domain.TicketTypes.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.Events.UseCases.Publish
{
    internal sealed class PublishEventHandler(IEventRepository eventRepository,
                                            ITicketTypeRepository tickeTypeRepository) : ICommandHandler<PublishEventCommand, PublishEventResponse>
    {
        public async Task<Result<PublishEventResponse>> ExecuteAsync(PublishEventCommand request, CancellationToken cancellationToken = default)
        {
            var @event = await eventRepository.GetByIdAsync(request.EventId, cancellationToken).ConfigureAwait(false);
            if (@event is null)
                return Result.Failure<PublishEventResponse>(EventErrors.NotFound(request.EventId));

            if (!await tickeTypeRepository.ExistsAsync(request.EventId))
                return Result.Failure<PublishEventResponse>(EventErrors.NoTicketsFound);

            @event.Publish();
            eventRepository.Update(@event);

            var saveChanges = await tickeTypeRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges ? Result.Success(new PublishEventResponse(@event.Id)) : Result.Failure<PublishEventResponse>(EventErrors.EventCanNotBePublished);
        }
    }
}