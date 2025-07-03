using Eventix.Modules.Attendance.Domain.Events.Entities;
using Eventix.Shared.Domain.Interfaces;

namespace Eventix.Modules.Attendance.Domain.Events.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        void Insert(Event @event);
    }
}