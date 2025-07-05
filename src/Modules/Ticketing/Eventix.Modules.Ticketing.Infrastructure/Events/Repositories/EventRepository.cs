using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Modules.Ticketing.Domain.Events.Interfaces;
using Eventix.Modules.Ticketing.Infrastructure.Database;
using Eventix.Shared.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Ticketing.Infrastructure.Events.Repositories
{
    internal sealed class EventRepository(TicketingDbContext context) : IEventRepository
    {
        public IUnitOfWork UnitOfWork => context;

        public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Events.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public void Insert(Event entity)
            => context.Events.Add(entity);

        public void Update(Event entity)
            => context.Events.Update(entity);

        public void Dispose()
            => context.Dispose();
    }
}