using Eventix.Modules.Attendance.Domain.Tickets.Entities;
using Eventix.Shared.Domain.Interfaces;

namespace Eventix.Modules.Attendance.Domain.Tickets.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        void Insert(Ticket ticket);

        void Update(Ticket ticket);
    }
}