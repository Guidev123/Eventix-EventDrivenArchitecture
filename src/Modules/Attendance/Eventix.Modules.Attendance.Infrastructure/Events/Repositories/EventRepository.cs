using Eventix.Modules.Attendance.Domain.Events.Entities;
using Eventix.Modules.Attendance.Domain.Events.Interfaces;
using Eventix.Modules.Attendance.Infrastructure.Database;
using Eventix.Shared.Domain.Interfaces;

namespace Eventix.Modules.Attendance.Infrastructure.Events.Repositories
{
    internal sealed class EventRepository(AttendanceDbContext context) : IEventRepository
    {
        public IUnitOfWork UnitOfWork => context;

        public Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Insert(Event @event)
        {
            throw new NotImplementedException();
        }

        public void Dispose() => context.Dispose();
    }
}