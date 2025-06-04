using Dapper;
using Eventix.Modules.Events.Domain.Events.Entities;
using Eventix.Modules.Events.Domain.Events.Interfaces;
using Eventix.Modules.Events.Infrastructure.Data;
using Eventix.Modules.Events.Infrastructure.Database;

namespace Eventix.Modules.Events.Infrastructure.Events
{
    public sealed class EventRepository(EventsDbContext context, DbConnectionFactory connectionFactory) : IEventRepository
    {
        public async Task<Event?> GetByIdAsync(Guid id)
        {
            using var connection = connectionFactory.Create();
            await connection.OpenAsync().ConfigureAwait(false);

            const string sql = @$"
                SELECT
                    Id,
                    Title,
                    Description,
                    Location,
                    StartsAtUtc,
                    EndsAtUtc
                FROM {Schemas.Events}.Events
                WHERE Id = @Id";

            var @event = await connection.QuerySingleOrDefaultAsync(sql, id).ConfigureAwait(false);
            if (@event is null) return null;

            return @event as Event;
        }

        public void Insert(Event @event)
            => context.Events.Add(@event);

        public void Dispose()
        {
            context.Dispose();
        }
    }
}