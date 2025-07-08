using Eventix.Modules.Events.Application.TicketTypes.DTOs;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.GetById
{
    public sealed record GetTicketTypeByIdQuery(Guid TicketTypeId) : IQuery<TicketTypeResponse>;
}