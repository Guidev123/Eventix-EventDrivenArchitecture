using Eventix.Modules.Events.Domain.Events.DomainEvents;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.DomainEvents
{
    public sealed class EventRescheduleDomainEventHandler : IDomainEventHandler<EventRescheduleDomainEvent>
    {
        public async Task ExecuteAsync(EventRescheduleDomainEvent notification, CancellationToken cancellationToken)
        {
            await Task.Delay(100);
        }
    }
}