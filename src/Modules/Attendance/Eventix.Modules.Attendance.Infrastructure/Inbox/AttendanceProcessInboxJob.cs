using Eventix.Modules.Attendance.Infrastructure.Database;
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

namespace Eventix.Modules.Attendance.Infrastructure.Inbox
{
    [DisallowConcurrentExecution]
    internal sealed class AttendanceProcessInboxJob(
        ISqlConnectionFactory sqlConnectionFactory,
        IServiceScopeFactory serviceScopeFactory,
        IDateTimeProvider dateTimeProvider,
        IOptions<InboxOptions> options,
        ILogger<AttendanceProcessInboxJob> logger) : ProcessInboxJob(dateTimeProvider), IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            logger.LogInformation("{Module} - Beginning to process inbox messages", Schemas.Attendance);
            using var connection = sqlConnectionFactory.Create();
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            var inboxMessages = await GetInboxMessagesAsync(connection, transaction, options.Value.BatchSize, Schemas.Attendance);

            foreach (var inboxMessage in inboxMessages)
            {
                Exception? exception = null;
                try
                {
                    var integrationEvent = JsonConvert.DeserializeObject<IIntegrationEvent>(inboxMessage.Content, SerializerExtension.Instance)!;

                    using var scope = serviceScopeFactory.CreateScope();

                    var integrationEventHandlers = IntegrationEventHandlersFactory.GetHandlers(
                        integrationEvent.GetType(),
                        scope.ServiceProvider,
                        typeof(AttendanceModule).Assembly);

                    foreach (var handler in integrationEventHandlers)
                    {
                        await handler.ExecuteAsync(integrationEvent);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "{Module} - Exception while processing inbox message {MessageId}", Schemas.Attendance, inboxMessage.Id);

                    exception = ex;
                }

                await UpdateInboxMessageAsync(connection, transaction, inboxMessage, exception, Schemas.Attendance);
            }

            await transaction.CommitAsync();

            logger.LogInformation("{Module} - Completed process inbox messages", Schemas.Attendance);
        }
    }
}