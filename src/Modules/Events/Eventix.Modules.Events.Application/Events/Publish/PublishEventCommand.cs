using Eventix.Modules.Events.Application.Abstractions.Messaging;

namespace Eventix.Modules.Events.Application.Events.Publish
{
    public record PublishEventCommand(Guid EventId) : ICommand<PublishEventResponse>
    {
    }
}