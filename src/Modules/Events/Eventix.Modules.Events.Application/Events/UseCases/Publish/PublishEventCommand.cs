using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.UseCases.Publish
{
    public record PublishEventCommand(Guid EventId) : ICommand<PublishEventResponse>
    {
    }
}