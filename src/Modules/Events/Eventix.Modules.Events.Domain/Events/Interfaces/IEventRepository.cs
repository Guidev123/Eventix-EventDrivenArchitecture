using Eventix.Modules.Events.Domain.Events.Entities;

namespace Eventix.Modules.Events.Domain.Events.Interfaces
{
    public interface IEventRepository : IDisposable
    {
        void Insert(Event @event);

        void Update(Event @event);

        Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}