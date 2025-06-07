using Eventix.Modules.Events.Domain.TicketTypes.Entities;

namespace Eventix.Modules.Events.Domain.TicketTypes.Interfaces
{
    public interface ITicketTypeRepository : IDisposable
    {
        Task<TicketType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken = default);

        void Update(TicketType ticketType);

        void Insert(TicketType ticketType);
    }
}