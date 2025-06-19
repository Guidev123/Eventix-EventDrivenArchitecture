using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Shared.Domain.Interfaces;

namespace Eventix.Modules.Ticketing.Domain.Events.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        void Insert(Event entity);

        Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}