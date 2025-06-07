using Eventix.Modules.Events.Application.Abstractions.Messaging;

namespace Eventix.Modules.Events.Application.TicketTypes.UpdatePrice
{
    public sealed record UpdateTicketTypePriceCommand : ICommand
    {
        public UpdateTicketTypePriceCommand(decimal price)
        {
            Price = price;
        }

        public Guid? TicketTypeId { get; private set; }
        public decimal Price { get; }
        public void SetTicketTypeId(Guid ticketTypeId) => TicketTypeId = ticketTypeId;
    }
}