using Eventix.Modules.Events.Domain.Evemts.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Events.Infrastructure.Database
{
    public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options) : DbContext(options)
    {
        public DbSet<Event> Events { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schemas.Events);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventsDbContext).Assembly);
        }
    }
}