using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Events.UseCases.Cancel
{
    public sealed record CancelEventCommand(Guid EventId) : ICommand;
}