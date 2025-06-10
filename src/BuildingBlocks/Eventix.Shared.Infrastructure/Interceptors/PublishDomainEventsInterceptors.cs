using Eventix.Shared.Domain.DomainEvents;
using Eventix.Shared.Domain.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using MidR.Interfaces;

namespace Eventix.Shared.Infrastructure.Interceptors
{
    public sealed class PublishDomainEventsInterceptors(IServiceScopeFactory serviceScopeFactory) : SaveChangesInterceptor
    {
        public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is not null)
                await PublishDomainEventsAsync(eventData.Context, cancellationToken);

            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        private async Task PublishDomainEventsAsync(DbContext context, CancellationToken cancellationToken)
        {
            var domainEvents = context
                     .ChangeTracker
                     .Entries<Entity>()
                     .Select(entry => entry.Entity)
                     .SelectMany(entity =>
                     {
                         IReadOnlyCollection<IDomainEvent> domainEvents = entity.DomainEvents.ToList();

                         entity.ClearDomainEvents();

                         return domainEvents;
                     })
                     .ToList();

            using IServiceScope scope = serviceScopeFactory.CreateScope();

            var publisher = scope.ServiceProvider.GetRequiredService<IMediator>();

            foreach (IDomainEvent domainEvent in domainEvents)
            {
                await publisher.NotifyAsync(domainEvent, cancellationToken);
            }
        }
    }
}