using Eventix.Modules.Attendance.Domain.Attendees.Entities;
using Eventix.Modules.Attendance.Domain.Attendees.Interfaces;
using Eventix.Modules.Attendance.Infrastructure.Database;
using Eventix.Shared.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Attendance.Infrastructure.Attendees.Repositories
{
    internal sealed class AttendeeRepository(AttendanceDbContext context) : IAttendeeRepository
    {
        public IUnitOfWork UnitOfWork => context;

        public void Dispose() => context.Dispose();

        public async Task<Attendee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Attendees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);

        public void Insert(Attendee attendee)
            => context.Attendees.Add(attendee);
    }
}