using Eventix.Modules.Attendance.Domain.Events.Entities;
using Eventix.Modules.Attendance.Domain.Events.Models;
using Eventix.Shared.Domain.Interfaces;

namespace Eventix.Modules.Attendance.Domain.Events.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        void Insert(Event @event);

        void InsertStatistics(EventStatistic statistics);

        void Delete(Event @event);

        void Update(Event @event);
    }
}