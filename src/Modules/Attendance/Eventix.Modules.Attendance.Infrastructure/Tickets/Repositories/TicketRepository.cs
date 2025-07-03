using Eventix.Modules.Attendance.Domain.Tickets.Entities;
using Eventix.Modules.Attendance.Domain.Tickets.Interfaces;
using Eventix.Modules.Attendance.Infrastructure.Database;
using Eventix.Shared.Domain.Interfaces;

namespace Eventix.Modules.Attendance.Infrastructure.Tickets.Repositories
{
    internal sealed class TicketRepository(AttendanceDbContext context) : ITicketRepository
    {
        public IUnitOfWork UnitOfWork => context;

        public Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Insert(Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public void Dispose() => context.Dispose();
    }
}