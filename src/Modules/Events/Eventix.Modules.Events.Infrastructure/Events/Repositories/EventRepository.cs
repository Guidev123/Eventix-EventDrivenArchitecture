using Eventix.Modules.Events.Domain.Events.Entities;
using Eventix.Modules.Events.Domain.Events.Interfaces;
using Eventix.Modules.Events.Infrastructure.Database;
using Eventix.Shared.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Events.Infrastructure.Events.Repositories
{
    public sealed class EventRepository(EventsDbContext context) : IEventRepository
    {
        public IUnitOfWork UnitOfWork => context;

        public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
                .ConfigureAwait(false);

        public void Insert(Event @event)
            => context.Events.Add(@event);

        public void Update(Event @event)
            => context.Events.Update(@event);

        public void Dispose() => context.Dispose();
    }
}