using Eventix.Modules.Attendance.Domain.Events.Entities;
using Eventix.Modules.Attendance.Domain.Events.Interfaces;
using Eventix.Modules.Attendance.Domain.Events.Models;
using Eventix.Modules.Attendance.Infrastructure.Database;
using Eventix.Shared.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Attendance.Infrastructure.Events.Repositories
{
    internal sealed class EventRepository(AttendanceDbContext context) : IEventRepository
    {
        public IUnitOfWork UnitOfWork => context;

        public Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => context.Events.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public void Insert(Event @event)
            => context.Events.Add(@event);

        public void InsertStatistics(EventStatistic statistics)
            => context.EventStatistics.Add(statistics);

        public void Update(Event @event)
            => context.Events.Update(@event);

        public void Delete(Event @event)
            => context.Events.Remove(@event);

        public void Dispose() => context.Dispose();
    }
}