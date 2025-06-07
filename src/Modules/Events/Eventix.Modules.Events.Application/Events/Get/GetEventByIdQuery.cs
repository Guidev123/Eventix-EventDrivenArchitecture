using Eventix.Modules.Events.Application.Abstractions.Messaging;

namespace Eventix.Modules.Events.Application.Events.Get
{
    public record GetEventByIdQuery(Guid EventId) : IQuery<GetEventResponse>
    {
    }
}