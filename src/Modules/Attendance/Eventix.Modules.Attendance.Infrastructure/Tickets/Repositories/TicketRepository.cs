using Eventix.Modules.Attendance.Domain.Tickets.Entities;
using Eventix.Modules.Attendance.Domain.Tickets.Interfaces;
using Eventix.Modules.Attendance.Infrastructure.Database;
using Eventix.Shared.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Attendance.Infrastructure.Tickets.Repositories
{
    internal sealed class TicketRepository(AttendanceDbContext context) : ITicketRepository
    {
        public IUnitOfWork UnitOfWork => context;

        public Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => context.Tickets.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public void Insert(Ticket ticket)
            => context.Tickets.Add(ticket);

        public void Dispose() => context.Dispose();
    }
}