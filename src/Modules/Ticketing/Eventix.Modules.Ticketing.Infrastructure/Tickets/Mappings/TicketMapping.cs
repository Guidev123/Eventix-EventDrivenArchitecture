using Eventix.Modules.Ticketing.Domain.Customers.Entities;
using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Modules.Ticketing.Domain.Orders.Entities;
using Eventix.Modules.Ticketing.Domain.Tickets.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Modules.Ticketing.Infrastructure.Tickets.Mappings
{
    internal sealed class TicketMapping : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Code).HasColumnType("VARCHAR(30)");

            builder.HasIndex(t => t.Code).IsUnique();

            builder.HasOne<Customer>().WithMany().HasForeignKey(t => t.CustomerId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Order>().WithMany().HasForeignKey(t => t.OrderId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Event>().WithMany().HasForeignKey(t => t.EventId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<TicketType>().WithMany().HasForeignKey(t => t.TicketTypeId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}