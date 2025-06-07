using Eventix.Modules.Events.Domain.Events.Entities;
using Eventix.Shared.Domain.Interfaces;

namespace Eventix.Modules.Events.Domain.Events.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        void Insert(Event @event);

        void Update(Event @event);

        Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}