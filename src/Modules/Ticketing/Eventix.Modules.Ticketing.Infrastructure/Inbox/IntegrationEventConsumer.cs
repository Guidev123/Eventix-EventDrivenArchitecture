using Dapper;
using Eventix.Modules.Events.IntegrationEvents.Events;
using Eventix.Modules.Events.IntegrationEvents.TicketTypes;
using Eventix.Modules.Ticketing.Infrastructure.Database;
using Eventix.Modules.Ticketing.IntegrationEvents.Payments;
using Eventix.Modules.Users.IntegrationEvents.Users;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Infrastructure.Extensions;
using Eventix.Shared.Infrastructure.Inbox.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Eventix.Modules.Ticketing.Infrastructure.Inbox
{
    internal sealed class IntegrationEventConsumer(
      IEventBus eventBus,
      ISqlConnectionFactory sqlConnectionFactory
      ) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers(stoppingToken);

            return Task.CompletedTask;
        }

        private async Task ConsumeAsync(IIntegrationEvent integrationEvent)
        {
            using var connection = sqlConnectionFactory.Create();

            var inboxMessage = new InboxMessage
            {
                Id = integrationEvent.CorrelationId,
                Content = JsonConvert.SerializeObject(integrationEvent, SerializerExtension.Instance),
                Type = integrationEvent.GetType().Name,
                OccurredOnUtc = integrationEvent.OccurredOnUtc
            };

            var sql = $@"
                INSERT INTO {Schemas.Ticketing}.InboxMessages (Id, Type, Content, OccurredOnUtc)
                VALUES (@Id, @Type, @Content, @OccurredOnUtc)";

            await connection.ExecuteAsync(sql, inboxMessage);
        }

        private void SetSubscribers(CancellationToken cancellationToken = default)
        {
            eventBus.SubscribeAsync<UserRegisteredIntegrationEvent>("ticketing-user-registered", ConsumeAsync, cancellationToken);
            eventBus.SubscribeAsync<UserUpdatedIntegrationEvent>("ticketing-user-updated", ConsumeAsync, cancellationToken);

            eventBus.SubscribeAsync<EventCancelledIntegrationEvent>("ticketing-event-cancelled", ConsumeAsync, cancellationToken);
            eventBus.SubscribeAsync<EventPublishedIntegrationEvent>("ticketing-event-published", ConsumeAsync, cancellationToken);
            eventBus.SubscribeAsync<EventRescheduledIntegrationEvent>("ticketing-event-rescheduled", ConsumeAsync, cancellationToken);

            eventBus.SubscribeAsync<TicketTypePriceChangedIntegrationEvent>("ticketing-ticket-type-price-changed", ConsumeAsync, cancellationToken);

            eventBus.SubscribeAsync<OrderPlacedIntegrationEvent>("ticketing-order-placed", ConsumeAsync, cancellationToken);
        }
    }
}