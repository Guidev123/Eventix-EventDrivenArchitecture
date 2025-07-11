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

        public async Task<Attendee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Attendees.FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);

        public void Insert(Attendee attendee)
            => context.Attendees.Add(attendee);

        public void Update(Attendee attendee)
            => context.Attendees.Update(attendee);

        public void Dispose() => context.Dispose();
    }
}