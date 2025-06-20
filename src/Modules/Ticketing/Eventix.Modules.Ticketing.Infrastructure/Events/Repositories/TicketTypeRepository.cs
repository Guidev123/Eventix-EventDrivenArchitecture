using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Modules.Ticketing.Domain.Events.Interfaces;
using Eventix.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Ticketing.Infrastructure.Events.Repositories
{
    internal sealed class TicketTypeRepository(TicketingDbContext context) : ITicketTypeRepository
    {
        public async Task<TicketType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.TicketTypes.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        public async Task<TicketType?> GetWithLockAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await context
                .TicketTypes
                .FromSql(
                    $@"
                    SELECT Id, EventId, Name, Price, Currency, Quantity, AvailableQuantity
                    FROM ticketing.TicketTypes
                    WITH (UPDLOCK, ROWLOCK)
                    WHERE Id = {id}")
                .SingleOrDefaultAsync(cancellationToken);
        }

        public void InsertRange(IEnumerable<TicketType> ticketTypes)
            => context.TicketTypes.AddRange(ticketTypes);

        public void Update(TicketType ticketType)
            => context.TicketTypes.Update(ticketType);

        public void Dispose() => context.Dispose();
    }
}