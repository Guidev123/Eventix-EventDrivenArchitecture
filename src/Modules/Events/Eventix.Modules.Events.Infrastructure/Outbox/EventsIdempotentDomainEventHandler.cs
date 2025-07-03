using Eventix.Modules.Events.Infrastructure.Database;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.DomainEvents;
using Eventix.Shared.Infrastructure.Outbox;
using Eventix.Shared.Infrastructure.Outbox.Models;

namespace Eventix.Modules.Events.Infrastructure.Outbox
{
    internal sealed class EventsIdempotentDomainEventHandler<TDomainEvent>(
        IDomainEventHandler<TDomainEvent> innerHandler,
        ISqlConnectionFactory sqlConnectionFactory) : IdempotentDomainEventHandler<TDomainEvent>
            where TDomainEvent : IDomainEvent
    {
        public override async Task ExecuteAsync(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();

            var outboxMessageConsumer = new OutboxMessageConsumer(domainEvent.Id, innerHandler.GetType().Name);

            if (await IsOutboxMessageProcessedAsync(outboxMessageConsumer, connection, Schemas.Events)) return;

            await innerHandler.ExecuteAsync(domainEvent, cancellationToken);

            await MarkOutboxMessageAsProcessedAsync(outboxMessageConsumer, connection, Schemas.Events);
        }
    }
}