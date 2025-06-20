using Eventix.Modules.Ticketing.Domain.Customers.Entities;
using Eventix.Modules.Ticketing.Domain.Orders.Entities;
using Eventix.Shared.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Modules.Ticketing.Infrastructure.Orders.Mappings
{
    internal sealed class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(c => c.Id);

            builder.OwnsOne(c => c.TotalPrice, totalPrice =>
            {
                totalPrice.Property(c => c.Amount)
                .HasColumnName("TotalPrice")
                .HasColumnType("MONEY");

                totalPrice.Property(c => c.Currency)
                .HasColumnName(nameof(Money.Currency))
                .HasColumnType("VARCHAR(10)");
            });

            builder.HasOne<Customer>().WithMany().HasForeignKey(o => o.CustomerId);

            builder.HasMany(o => o.OrderItems).WithOne().HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}