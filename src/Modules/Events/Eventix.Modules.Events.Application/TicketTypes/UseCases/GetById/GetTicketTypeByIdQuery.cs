using Eventix.Modules.Events.Application.TicketTypes.Dtos;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.GetById
{
    public sealed record GetTicketTypeByIdQuery(Guid TicketTypeId) : IQuery<TicketTypeDto>;
}