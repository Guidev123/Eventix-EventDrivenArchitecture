using Eventix.Modules.Users.Infrastructure.Database;
using Eventix.Shared.Application.Clock;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Domain.DomainEvents;
using Eventix.Shared.Infrastructure.Extensions;
using Eventix.Shared.Infrastructure.Outbox;
using Eventix.Shared.Infrastructure.Outbox.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Eventix.Modules.Users.Infrastructure.Outbox
{
    [DisallowConcurrentExecution]
    internal sealed class UsersProcessOutboxJob(
        ISqlConnectionFactory sqlConnectionFactory,
        IServiceScopeFactory serviceScopeFactory,
        IDateTimeProvider dateTimeProvider,
        IOptions<OutboxOptions> options,
        ILogger<UsersProcessOutboxJob> logger) : ProcessOutboxJob(dateTimeProvider), IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            logger.LogInformation("{Module} - Beginning to process outbox messages", Schemas.Users);
            using var connection = sqlConnectionFactory.Create();
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            var outboxMessages = await GetOutboxMessagesAsync(connection, transaction, options.Value.BatchSize, Schemas.Users);

            foreach (var outboxMessage in outboxMessages)
            {
                Exception? exception = null;
                try
                {
                    var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, SerializerExtension.Instance)!;

                    using var scope = serviceScopeFactory.CreateScope();

                    var domainEventHandlers = DomainEventHandlersFactory.GetHandlers(
                        domainEvent.GetType(),
                        scope.ServiceProvider,
                        Application.AssemblyReference.Assembly);

                    foreach (var handler in domainEventHandlers)
                    {
                        await handler.ExecuteAsync(domainEvent);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "{Module} - Exception while processing outbox message {MessageId}", Schemas.Users, outboxMessage.Id);

                    exception = ex;
                }

                await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception, Schemas.Users);
            }

            await transaction.CommitAsync();

            logger.LogInformation("{Module} - Completed process outbox messages", Schemas.Users);
        }
    }
}