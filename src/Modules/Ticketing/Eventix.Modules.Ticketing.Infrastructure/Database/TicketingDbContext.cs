using Eventix.Modules.Ticketing.Domain.Customers.Entities;
using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Modules.Ticketing.Domain.Orders.Entities;
using Eventix.Modules.Ticketing.Domain.Payments.Entities;
using Eventix.Modules.Ticketing.Domain.Tickets.Entities;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Infrastructure.Outbox.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Eventix.Modules.Ticketing.Infrastructure.Database
{
    public sealed class TicketingDbContext(DbContextOptions<TicketingDbContext> options) : DbContext(options), IUnitOfWork
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<TicketType> TicketTypes { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schemas.Ticketing);

            modelBuilder.ApplyConfiguration(new OutboxMessageMapping());
            modelBuilder.ApplyConfiguration(new OutboxMessageConsumerMapping());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
            => await SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
    }
}