using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.TicketTypes.UseCases.UpdatePrice
{
    public record UpdateTicketTypePriceCommand(Guid TicketTypeId, decimal Price) : ICommand;
}