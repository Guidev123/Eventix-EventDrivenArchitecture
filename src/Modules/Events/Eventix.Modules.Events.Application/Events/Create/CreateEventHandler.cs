using Eventix.Modules.Events.Application.Abstractions.Data;
using Eventix.Modules.Events.Domain.Events.Interfaces;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Application.Events.Create
{
    public sealed class CreateEventHandler(IEventRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateEventCommand, CreateEventResponse>
    {
        public async Task<CreateEventResponse> ExecuteAsync(CreateEventCommand request, CancellationToken cancellationToken = default)
        {
            var @event = CreateEventCommand.ToEvent(request);

            repository.Insert(@event);

            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return new(@event.Id);
        }
    }
}