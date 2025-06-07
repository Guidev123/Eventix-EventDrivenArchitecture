using Eventix.Modules.Events.Application.Abstractions.Messaging;
using Eventix.Modules.Events.Domain.TicketTypes.Entities;

namespace Eventix.Modules.Events.Application.TicketTypes.Create
{
    public record CreateTicketTypeCommand(
        Guid EventId,
        string Name,
        decimal Price,
        string Currency,
        decimal Quantity
        ) : ICommand<CreateTicketTypeResponse>
    {
        public static TicketType ToTicketType(CreateTicketTypeCommand command)
            => TicketType.Create(command.EventId, command.Name, command.Price, command.Currency, command.Quantity);
    }
}