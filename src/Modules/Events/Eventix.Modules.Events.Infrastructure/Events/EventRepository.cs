using Eventix.Modules.Events.Domain.Events.Entities;
using Eventix.Modules.Events.Domain.Events.Interfaces;
using Eventix.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Events.Infrastructure.Events
{
    public sealed class EventRepository(EventsDbContext context) : IEventRepository
    {
        public async Task<Event?> GetByIdAsync(Guid id)
            => await context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id)
                .ConfigureAwait(false);

        public void Insert(Event @event)
            => context.Events.Add(@event);

        public void Dispose()
        {
            context.Dispose();
        }
    }
}