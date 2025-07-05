using Eventix.Modules.Attendance.Domain.Attendees.Entities;
using Eventix.Shared.Domain.Interfaces;

namespace Eventix.Modules.Attendance.Domain.Attendees.Interfaces
{
    public interface IAttendeeRepository : IRepository<Attendee>
    {
        Task<Attendee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        void Insert(Attendee attendee);

        void Update(Attendee attendee);
    }
}