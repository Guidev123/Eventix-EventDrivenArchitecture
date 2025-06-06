using Eventix.Modules.Events.Application.Abstractions.Messaging;

namespace Eventix.Modules.Events.Application.Events.Cancel
{
    public record CancelEventCommand(Guid EventId) : ICommand;
}