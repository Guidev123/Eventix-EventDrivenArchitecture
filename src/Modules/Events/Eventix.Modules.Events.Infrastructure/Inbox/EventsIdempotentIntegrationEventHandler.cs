using Eventix.Modules.Events.Infrastructure.Database;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Infrastructure.Inbox;
using Eventix.Shared.Infrastructure.Inbox.Models;

namespace Eventix.Modules.Events.Infrastructure.Inbox
{
    internal sealed class EventsIdempotentIntegrationEventHandler<TIntegrationEvent>(
        IIntegrationEventHandler<TIntegrationEvent> innerHandler,
        ISqlConnectionFactory sqlConnectionFactory) : IdempotentIntegrationEventHandler<TIntegrationEvent>
            where TIntegrationEvent : IIntegrationEvent
    {
        public override async Task ExecuteAsync(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();

            var inboxMessageConsumer = new InboxMessageConsumer(integrationEvent.Id, innerHandler.GetType().Name);

            if (await IsInboxMessageProcessedAsync(inboxMessageConsumer, connection, Schemas.Events)) return;

            await innerHandler.ExecuteAsync(integrationEvent, cancellationToken);

            await MarkInboxMessageAsProcessedAsync(inboxMessageConsumer, connection, Schemas.Events);
        }
    }
}