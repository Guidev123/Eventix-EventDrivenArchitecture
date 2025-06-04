using Eventix.Modules.Events.Application.Abstractions.Data;
using Eventix.Modules.Events.Domain.Events.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Events.Infrastructure.Database
{
    public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options) : DbContext(options), IUnitOfWork
    {
        public DbSet<Event> Events { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schemas.Events);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventsDbContext).Assembly);
        }
    }
}