using Bogus;
using Eventix.Modules.Events.Infrastructure.Database;
using Eventix.Shared.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.Events.IntegrationTests.Abstractions
{
    [Collection(nameof(IntegrationTestCollection))]
    public class BaseIntegrationTest : IDisposable
    {
        private readonly IServiceScope _serviceScope;
        protected readonly IMediatorHandler _mediatorHandler;
        protected static readonly Faker _faker = new();
        protected readonly IntegrationWebApplicationFactory _factory;
        protected readonly EventsDbContext _dbContext;

        protected BaseIntegrationTest(IntegrationWebApplicationFactory factory)
        {
            _factory = factory;
            _serviceScope = factory.Services.CreateScope();
            _mediatorHandler = _serviceScope.ServiceProvider.GetRequiredService<IMediatorHandler>();
            _dbContext = _serviceScope.ServiceProvider.GetRequiredService<EventsDbContext>();
        }

        protected async Task CleanDatabaseAsync()
        {
            await _dbContext.Database.ExecuteSqlRawAsync(
                """
            DELETE FROM events.InboxMessageConsumers;
            DELETE FROM events.InboxMessages;
            DELETE FROM events.OutboxMessageConsumers;
            DELETE FROM events.OutboxMessages;
            DELETE FROM events.TicketTypes;
            DELETE FROM events.Events;
            DELETE FROM events.Categories;
            """);
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
        }
    }
}