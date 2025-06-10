using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.GetById
{
    public record GetTicketTypeByIdQuery(Guid TicketTypeId) : IQuery<GetTicketTypeResponse>;
}