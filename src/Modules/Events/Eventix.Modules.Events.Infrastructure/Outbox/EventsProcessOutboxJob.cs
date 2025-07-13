using Eventix.Modules.Events.Infrastructure.Database;
using Eventix.Shared.Application.Clock;
using Eventix.Shared.Application.EventSourcing;
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

namespace Eventix.Modules.Events.Infrastructure.Outbox
{
    [DisallowConcurrentExecution]
    internal sealed class EventsProcessOutboxJob(
        ISqlConnectionFactory sqlConnectionFactory,
        IServiceScopeFactory serviceScopeFactory,
        IDateTimeProvider dateTimeProvider,
        IOptions<OutboxOptions> options,
        IEventSourcingRepository eventSourcingRepository,
        ILogger<EventsProcessOutboxJob> logger) : ProcessOutboxJob(dateTimeProvider), IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            logger.LogInformation("{Module} - Beginning to process outbox messages", Schemas.Events);
            using var connection = sqlConnectionFactory.Create();
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            var outboxMessages = await GetOutboxMessagesAsync(connection, transaction, options.Value.BatchSize, Schemas.Events);

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

                    var handlerTasks = domainEventHandlers.Select(handler => handler.ExecuteAsync(domainEvent));
                    await Task.WhenAll(handlerTasks);

                    if (domainEvent.AggregateId != Guid.Empty)
                        await eventSourcingRepository.SaveAsync(domainEvent);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "{Module} - Exception while processing outbox message {MessageId}", Schemas.Events, outboxMessage.Id);

                    exception = ex;
                }

                await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception, Schemas.Events);
            }

            await transaction.CommitAsync();

            logger.LogInformation("{Module} - Completed process outbox messages", Schemas.Events);
        }
    }
}