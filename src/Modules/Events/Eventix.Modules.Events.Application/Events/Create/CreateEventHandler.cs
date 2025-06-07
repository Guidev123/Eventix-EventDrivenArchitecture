using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.Domain.Events.Interfaces;
using Eventix.Modules.Events.Domain.Shared.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.Events.Create
{
    public sealed class CreateEventHandler(IEventRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<CreateEventCommand, CreateEventResponse>
    {
        public async Task<Result<CreateEventResponse>> ExecuteAsync(CreateEventCommand request, CancellationToken cancellationToken = default)
        {
            var @event = CreateEventCommand.ToEvent(request);

            repository.Insert(@event);

            var rows = await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            if (rows < 0)
                return Result.Failure<CreateEventResponse>(EventErrors.UnableToCreateEvent);

            return Result.Success(new CreateEventResponse(@event.Id));
        }
    }
}