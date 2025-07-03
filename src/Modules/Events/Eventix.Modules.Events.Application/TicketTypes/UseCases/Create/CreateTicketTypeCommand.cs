using Eventix.Modules.Events.Domain.TicketTypes.Entities;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.Create
{
    public sealed record CreateTicketTypeCommand(
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