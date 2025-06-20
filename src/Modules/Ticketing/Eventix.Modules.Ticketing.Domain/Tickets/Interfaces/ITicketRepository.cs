using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Modules.Ticketing.Domain.Orders.Entities;
using Eventix.Modules.Ticketing.Domain.Tickets.Entities;
using Eventix.Shared.Domain.Interfaces;

namespace Eventix.Modules.Ticketing.Domain.Tickets.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<IEnumerable<Ticket>> GetForEventAsync(Event @event, CancellationToken cancellationToken = default);

        Task<IEnumerable<Ticket>> GetByOrderId(Guid orderId, CancellationToken cancellationToken = default);

        Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Ticket?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

        void InsertRange(IEnumerable<Ticket> tickets);
    }
}