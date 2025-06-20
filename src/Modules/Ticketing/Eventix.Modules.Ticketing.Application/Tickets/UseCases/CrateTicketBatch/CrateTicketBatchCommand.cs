using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.CrateTicketBatch
{
    public record CreateTicketBatchCommand(Guid OrderId) : ICommand;
}