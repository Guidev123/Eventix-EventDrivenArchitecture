using Eventix.Modules.Events.Application.Abstractions.Messaging;

namespace Eventix.Modules.Events.Application.TicketTypes.GetById
{
    public record GetTicketTypeByIdQuery(Guid TicketTypeId) : IQuery<GetTicketTypeByIdResponse>;
}