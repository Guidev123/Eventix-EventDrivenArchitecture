using Eventix.Modules.Ticketing.Application.Tickets.DTOs;
using Eventix.Modules.Ticketing.Domain.Tickets.Entities;

namespace Eventix.Modules.Ticketing.Application.Tickets.Mappers
{
    internal static class TicketMappers
    {
        public static TicketResponse MapFromTicket(this Ticket ticket)
            => new(
                ticket.Id, ticket.CustomerId,
                ticket.OrderId, ticket.EventId,
                ticket.TicketTypeId, ticket.Code,
                ticket.CreatedAtUtc
                );
    }
}