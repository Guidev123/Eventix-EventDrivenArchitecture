using Bogus;
using Eventix.Modules.Attendance.Infrastructure.Database;
using Eventix.Shared.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.Attendance.IntegrationTests.Abstractions
{
    [Collection(nameof(IntegrationTestCollection))]
    public class BaseIntegrationTest : IDisposable
    {
        private readonly IServiceScope _serviceScope;
        protected readonly IMediatorHandler _mediatorHandler;
        protected static readonly Faker _faker = new();
        protected readonly IntegrationWebApplicationFactory _factory;
        protected readonly AttendanceDbContext _dbContext;

        protected BaseIntegrationTest(IntegrationWebApplicationFactory factory)
        {
            _factory = factory;
            _serviceScope = factory.Services.CreateScope();
            _mediatorHandler = _serviceScope.ServiceProvider.GetRequiredService<IMediatorHandler>();
            _dbContext = _serviceScope.ServiceProvider.GetRequiredService<AttendanceDbContext>();
        }

        protected async Task CleanDatabaseAsync()
        {
            await _dbContext.Database.ExecuteSqlRawAsync(
                """
            DELETE FROM Attendance.InboxMessageConsumers;
            DELETE FROM Attendance.InboxMessages;
            DELETE FROM Attendance.OutboxMessageConsumers;
            DELETE FROM Attendance.OutboxMessages;
            DELETE FROM Attendance.Attendees;
            DELETE FROM Attendance.Events;
            DELETE FROM Attendance.Tickets;
            DELETE FROM Attendance.Event_statistics;
            """);
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
        }
    }
}