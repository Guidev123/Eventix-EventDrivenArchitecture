using Dapper;
using Eventix.Modules.Events.Infrastructure.Database;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Infrastructure.Inbox.Models;
using Eventix.Shared.Infrastructure.Serialization;
using MassTransit;
using Newtonsoft.Json;

namespace Eventix.Modules.Events.Infrastructure.Inbox
{
    internal sealed class IntegrationEventConsumer<TIntegrationEvent>(ISqlConnectionFactory sqlConnectionFactory)
            : IConsumer<TIntegrationEvent>
            where TIntegrationEvent : IntegrationEvent
    {
        public async Task Consume(ConsumeContext<TIntegrationEvent> context)
        {
            using var connection = sqlConnectionFactory.Create();

            var integrationEvent = context.Message;

            var inboxMessage = new InboxMessage
            {
                Id = integrationEvent.Id,
                Content = JsonConvert.SerializeObject(integrationEvent, SerializerSettings.Instance),
                Type = integrationEvent.GetType().Name,
                OccurredOnUtc = integrationEvent.OccurredOnUtc
            };

            var sql = $@"
                INSERT INTO {Schemas.Events}.InboxMessages (Id, Type, Content, OccurredOnUtc)
                VALUES (@Id, @Type, @Content, @OccurredOnUtc)";

            await connection.ExecuteAsync(sql, inboxMessage);
        }
    }
}