﻿using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Attendance.Domain.Attendees.DomainEvents
{
    public sealed record DuplicateCheckInAttemptDomainEvent(
        Guid AttendeeId,
        Guid EventId,
        Guid TicketId,
        string TicketCode
        ) : DomainEvent(AttendeeId);
}