using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Modules.Ticketing.Domain.Tickets.Entities;
using Eventix.Modules.Ticketing.Domain.Tickets.Interfaces;
using Eventix.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Ticketing.Infrastructure.Tickets.Repositories
{
    internal sealed class TicketRepository(TicketingDbContext context) : ITicketRepository
    {
        public async Task<Ticket?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
            => await context.Tickets.AsNoTracking().FirstOrDefaultAsync(c => c.Code == code, cancellationToken);

        public async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Tickets.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task<IEnumerable<Ticket>> GetByOrderId(Guid orderId, CancellationToken cancellationToken = default)
            => await context.Tickets.AsNoTracking().Where(x => x.OrderId == orderId).ToListAsync(cancellationToken);

        public async Task<IEnumerable<Ticket>> GetForEventAsync(Event @event, CancellationToken cancellationToken = default)
            => await context.Tickets.Where(t => t.EventId == @event.Id).ToListAsync(cancellationToken);

        public void InsertRange(IEnumerable<Ticket> tickets)
            => context.Tickets.AddRange(tickets);

        public void Dispose() => context.Dispose();
    }
}