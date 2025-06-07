using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.Cancel
{
    public record CancelEventCommand(Guid EventId) : ICommand;
}