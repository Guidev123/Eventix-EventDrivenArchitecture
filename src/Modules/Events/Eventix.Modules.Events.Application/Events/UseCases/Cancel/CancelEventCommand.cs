using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.UseCases.Cancel
{
    public sealed record CancelEventCommand(Guid EventId) : ICommand;
}