using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.Get
{
    public record GetEventByIdQuery(Guid EventId) : IQuery<GetEventResponse>
    {
    }
}