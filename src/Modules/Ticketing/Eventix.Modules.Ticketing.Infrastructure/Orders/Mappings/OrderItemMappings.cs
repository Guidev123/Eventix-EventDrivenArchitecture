using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Modules.Ticketing.Domain.Orders.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Modules.Ticketing.Infrastructure.Orders.Mappings
{
    internal sealed class OrderItemMappings : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(c => c.Id);

            builder.OwnsOne(c => c.Price, price =>
            {
                price.Property(c => c.Amount)
                .HasColumnName("Price")
                .HasColumnType("MONEY");

                price.Property(c => c.Currency)
                .HasColumnName("PriceCurrency")
                .HasColumnType("VARCHAR(10)");
            });

            builder.OwnsOne(c => c.UnitPrice, unitPrice =>
            {
                unitPrice.Property(c => c.Amount)
                .HasColumnName("UnitPrice")
                .HasColumnType("MONEY");

                unitPrice.Property(c => c.Currency)
                .HasColumnName("UnitPriceCurrency")
                .HasColumnType("VARCHAR(10)");
            });

            builder.OwnsOne(c => c.Quantity, quantity =>
            {
                quantity.Property(c => c.Value)
                .HasColumnName("Quantity");
            });

            builder.HasOne<TicketType>().WithMany().HasForeignKey(oi => oi.TicketTypeId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}