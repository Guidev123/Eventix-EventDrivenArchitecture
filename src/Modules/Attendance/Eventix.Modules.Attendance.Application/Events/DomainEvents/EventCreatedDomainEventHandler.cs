using Eventix.Modules.Attendance.Domain.Events.DomainEvents;
using Eventix.Modules.Attendance.Domain.Events.Errors;
using Eventix.Modules.Attendance.Domain.Events.Interfaces;
using Eventix.Modules.Attendance.Domain.Events.Models;
using Eventix.Shared.Application.Exceptions;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Attendance.Application.Events.DomainEvents
{
    internal sealed class EventCreatedDomainEventHandler(IEventRepository eventRepository)
        : DomainEventHandler<EventCreatedDomainEvent>
    {
        public override async Task ExecuteAsync(EventCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var @event = await eventRepository.GetByIdAsync(domainEvent.EventId, cancellationToken);
            if (@event is null)
                throw new EventixException(nameof(EventCreatedDomainEvent), EventErrors.NotFound(domainEvent.EventId));

            var eventStatistics = EventStatistic.Create(
                domainEvent.EventId,
                @event.Specification.Title,
                @event.Specification.Description,
                @event.Location,
                @event.DateRange.StartsAtUtc,
                @event.DateRange.EndsAtUtc);

            eventRepository.InsertStatistics(eventStatistics);

            var saveChanges = await eventRepository.UnitOfWork.CommitAsync(cancellationToken);
            if (!saveChanges)
                throw new EventixException(nameof(EventStatistic), EventErrors.FailToCreateEventStatistics);
        }
    }
}