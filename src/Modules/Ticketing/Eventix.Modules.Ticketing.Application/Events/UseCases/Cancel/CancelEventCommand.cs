using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Events.UseCases.Cancel
{
    public record CancelEventCommand(Guid EventId) : ICommand;
}