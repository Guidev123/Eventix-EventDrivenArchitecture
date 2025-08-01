using Eventix.Modules.Events.Infrastructure.Database;
using Eventix.Shared.Application.Clock;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Infrastructure.Extensions;
using Eventix.Shared.Infrastructure.Inbox;
using Eventix.Shared.Infrastructure.Inbox.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Eventix.Modules.Events.Infrastructure.Inbox
{
    [DisallowConcurrentExecution]
    internal sealed class EventsProcessInboxJob(
        ISqlConnectionFactory sqlConnectionFactory,
        IServiceScopeFactory serviceScopeFactory,
        IDateTimeProvider dateTimeProvider,
        IOptions<InboxOptions> options,
        ILogger<EventsProcessInboxJob> logger) : ProcessInboxJob(dateTimeProvider), IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            logger.LogInformation("{Module} - Beginning to process inbox messages", Schemas.Events);
            using var connection = sqlConnectionFactory.Create();
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            var inboxMessages = await GetInboxMessagesAsync(connection, transaction, options.Value.BatchSize, Schemas.Events);

            foreach (var inboxMessage in inboxMessages)
            {
                Exception? exception = null;
                try
                {
                    var integrationEvent = JsonConvert.DeserializeObject<IIntegrationEvent>(inboxMessage.Content, SerializerExtensions.Instance)!;

                    using var scope = serviceScopeFactory.CreateScope();

                    var integrationEventHandlers = IntegrationEventHandlersFactory.GetHandlers(
                        integrationEvent.GetType(),
                        scope.ServiceProvider,
                        typeof(EventsModule).Assembly);

                    foreach (var handler in integrationEventHandlers)
                    {
                        await handler.ExecuteAsync(integrationEvent);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "{Module} - Exception while processing inbox message {MessageId}", Schemas.Events, inboxMessage.Id);

                    exception = ex;
                }

                await UpdateInboxMessageAsync(connection, transaction, inboxMessage, exception, Schemas.Events);
            }

            await transaction.CommitAsync();

            logger.LogInformation("{Module} - Completed process inbox messages", Schemas.Events);
        }
    }
}