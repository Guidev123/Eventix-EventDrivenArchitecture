using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.Domain.Events.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.Events.UseCases.Create
{
    public sealed class CreateEventHandler(IEventRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<CreateEventCommand, CreateEventResponse>
    {
        public async Task<Result<CreateEventResponse>> ExecuteAsync(CreateEventCommand request, CancellationToken cancellationToken = default)
        {
            var @event = CreateEventCommand.ToEvent(request);

            repository.Insert(@event);

            var saveChanges = await unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges ? Result.Success(new CreateEventResponse(@event.Id)) : Result.Failure<CreateEventResponse>(EventErrors.UnableToCreateEvent);
        }
    }
}