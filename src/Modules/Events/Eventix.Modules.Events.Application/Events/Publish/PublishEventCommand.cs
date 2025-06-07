using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.Publish
{
    public record PublishEventCommand(Guid EventId) : ICommand<PublishEventResponse>
    {
    }
}