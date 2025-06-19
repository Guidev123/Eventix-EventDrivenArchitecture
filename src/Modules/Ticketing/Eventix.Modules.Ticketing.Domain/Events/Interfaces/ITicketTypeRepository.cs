using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Shared.Domain.Interfaces;

namespace Eventix.Modules.Ticketing.Domain.Events.Interfaces
{
    public interface ITicketTypeRepository : IRepository<TicketType>
    {
        Task<TicketType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}