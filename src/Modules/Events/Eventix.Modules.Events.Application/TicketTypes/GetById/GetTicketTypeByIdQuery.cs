using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.TicketTypes.GetById
{
    public record GetTicketTypeByIdQuery(Guid TicketTypeId) : IQuery<GetTicketTypeResponse>;
}