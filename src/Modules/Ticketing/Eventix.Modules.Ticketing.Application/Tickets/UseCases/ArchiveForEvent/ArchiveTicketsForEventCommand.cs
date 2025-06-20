using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.ArchiveForEvent
{
    public record ArchiveTicketsForEventCommand(Guid EventId) : ICommand;
}