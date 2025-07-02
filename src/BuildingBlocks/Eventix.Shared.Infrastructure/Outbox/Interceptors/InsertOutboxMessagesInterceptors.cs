using Eventix.Shared.Domain.DomainEvents;
using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Infrastructure.Outbox.Models;
using Eventix.Shared.Infrastructure.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace Eventix.Shared.Infrastructure.Outbox.Interceptors
{
    public sealed class InsertOutboxMessagesInterceptors : SaveChangesInterceptor
    {
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is not null)
                InsertOutboxMessages(eventData.Context, cancellationToken);

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void InsertOutboxMessages(DbContext context, CancellationToken cancellationToken)
        {
            var outboxMessages = context
                     .ChangeTracker
                     .Entries<Entity>()
                     .Select(entry => entry.Entity)
                     .SelectMany(entity =>
                     {
                         IReadOnlyCollection<IDomainEvent> domainEvents = entity.DomainEvents.ToList();

                         entity.ClearDomainEvents();

                         return domainEvents;
                     })
                     .Select(domainEvent => new OutboxMessage
                     {
                         Id = domainEvent.Id,
                         OccurredOnUtc = domainEvent.OccurredOnUtc,
                         Type = domainEvent.GetType().Name,
                         Content = JsonConvert.SerializeObject(domainEvent, SerializerSettings.Instance)
                     })
                     .ToList();

            context.Set<OutboxMessage>().AddRange(outboxMessages);
        }
    }
}