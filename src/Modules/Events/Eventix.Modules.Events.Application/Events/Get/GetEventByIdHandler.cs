using Eventix.Modules.Events.Domain.Events.Interfaces;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Application.Events.Get
{
    public sealed class GetEventByIdHandler(IEventRepository repository) : IRequestHandler<GetEventByIdQuery, GetEventByIdResponse?>
    {
        public async Task<GetEventByIdResponse?> ExecuteAsync(GetEventByIdQuery request, CancellationToken cancellationToken = default)
        {
            var @event = await repository.GetByIdAsync(request.EventId).ConfigureAwait(false);
            if (@event is null) return null;

            return GetEventByIdQuery.FromEvent(@event);
        }
    }
}