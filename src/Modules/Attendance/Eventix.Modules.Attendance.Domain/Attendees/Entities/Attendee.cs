using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Attendance.Domain.Attendees.Entities
{
    public sealed class Attendee : Entity, IAggregateRoot
    {
        protected override void Validate()
        {
        }
    }
}