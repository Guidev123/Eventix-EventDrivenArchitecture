using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.CrateTicketBatch
{
    public sealed record CreateTicketBatchCommand(Guid OrderId) : ICommand;
}