using Eventix.Modules.Events.Domain.TicketTypes.Entities;
using Eventix.Modules.Events.Domain.TicketTypes.Interfaces;
using Eventix.Modules.Events.Infrastructure.Database;
using Eventix.Shared.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Events.Infrastructure.TicketTypes.Repositories
{
    internal sealed class TicketTypeRepository(EventsDbContext context) : ITicketTypeRepository
    {
        public IUnitOfWork UnitOfWork => context;

        public async Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken = default)
            => await context.TicketTypes.AsNoTracking().Where(t => t.EventId == eventId)
                .AnyAsync(cancellationToken);

        public async Task<TicketType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.TicketTypes.AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        public void Insert(TicketType ticketType)
            => context.TicketTypes.Add(ticketType);

        public void Update(TicketType ticketType)
            => context.TicketTypes.Update(ticketType);

        public void Dispose()
            => context.Dispose();
    }
}