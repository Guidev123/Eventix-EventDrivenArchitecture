using Eventix.Modules.Attendance.Domain.Attendees.Entities;
using Eventix.Modules.Attendance.Domain.Attendees.Interfaces;
using Eventix.Modules.Attendance.Infrastructure.Database;
using Eventix.Shared.Domain.Interfaces;

namespace Eventix.Modules.Attendance.Infrastructure.Attendees.Repositories
{
    internal sealed class AttendeeRepository(AttendanceDbContext context) : IAttendeeRepository
    {
        public IUnitOfWork UnitOfWork => context;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<Attendee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Insert(Attendee attendee)
        {
            throw new NotImplementedException();
        }
    }
}