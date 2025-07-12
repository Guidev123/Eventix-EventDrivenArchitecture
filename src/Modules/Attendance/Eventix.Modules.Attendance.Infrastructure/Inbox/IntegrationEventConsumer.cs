using Dapper;
using Eventix.Modules.Attendance.Infrastructure.Database;
using Eventix.Modules.Events.IntegrationEvents.Events;
using Eventix.Modules.Ticketing.IntegrationEvents.Tickets;
using Eventix.Modules.Users.IntegrationEvents.Users;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Infrastructure.Extensions;
using Eventix.Shared.Infrastructure.Inbox.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Eventix.Modules.Attendance.Infrastructure.Inbox
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
                Id = integrationEvent.Id,
                Content = JsonConvert.SerializeObject(integrationEvent, SerializerExtension.Instance),
                Type = integrationEvent.GetType().Name,
                OccurredOnUtc = integrationEvent.OccurredOnUtc
            };

            var sql = $@"
                INSERT INTO {Schemas.Attendance}.InboxMessages (Id, Type, Content, OccurredOnUtc)
                VALUES (@Id, @Type, @Content, @OccurredOnUtc)";

            await connection.ExecuteAsync(sql, inboxMessage);
        }

        private void SetSubscribers(CancellationToken cancellationToken = default)
        {
            eventBus.SubscribeAsync<UserRegisteredIntegrationEvent>("attendance-user-registered", ConsumeAsync, cancellationToken);
            eventBus.SubscribeAsync<UserUpdatedIntegrationEvent>("attendance-user-updated", ConsumeAsync, cancellationToken);

            eventBus.SubscribeAsync<EventRescheduledIntegrationEvent>("attendance-event-rescheduled", ConsumeAsync, cancellationToken);
            eventBus.SubscribeAsync<EventPublishedIntegrationEvent>("attendance-event-published", ConsumeAsync, cancellationToken);
            eventBus.SubscribeAsync<EventCancelledIntegrationEvent>("attendance-event-cancelled", ConsumeAsync, cancellationToken);

            eventBus.SubscribeAsync<TicketCreatedIntegrationEvent>("attendance-ticket-created", ConsumeAsync, cancellationToken);
        }
    }
}