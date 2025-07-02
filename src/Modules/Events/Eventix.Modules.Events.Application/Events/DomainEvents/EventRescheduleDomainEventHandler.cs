using Eventix.Modules.Events.Domain.Events.DomainEvents;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.DomainEvents
{
    public sealed class EventRescheduleDomainEventHandler : DomainEventHandler<EventRescheduledDomainEvent>
    {
        public override async Task ExecuteAsync(EventRescheduledDomainEvent notification, CancellationToken cancellationToken = default)
        {
            await Task.Delay(100, cancellationToken);
        }
    }
}