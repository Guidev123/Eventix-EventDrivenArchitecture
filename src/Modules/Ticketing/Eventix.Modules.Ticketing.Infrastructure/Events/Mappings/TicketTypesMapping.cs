using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Modules.Ticketing.Domain.Events.ValueObjects;
using Eventix.Shared.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Modules.Ticketing.Infrastructure.Events.Mappings
{
    internal sealed class TicketTypesMapping : IEntityTypeConfiguration<TicketType>
    {
        public void Configure(EntityTypeBuilder<TicketType> builder)
        {
            builder.ToTable("TicketTypes");

            builder.HasKey(t => t.Id);

            builder.OwnsOne(t => t.Specification, spec =>
            {
                spec.Property(s => s.Name)
                     .HasColumnName(nameof(TicketTypeSpecification.Name))
                     .HasColumnType("VARCHAR(80)")
                     .IsRequired();

                spec.Property(s => s.Quantity)
                    .HasColumnName(nameof(TicketTypeSpecification.Quantity));

                spec.Property(s => s.AvailableQuantity)
                    .HasColumnName(nameof(TicketTypeSpecification.AvailableQuantity))
                    .HasColumnType("MONEY");
            });

            builder.OwnsOne(t => t.Price, price =>
            {
                price.Property(p => p.Amount)
                     .HasColumnName("Price")
                     .HasColumnType("MONEY");

                price.Property(p => p.Currency)
                     .HasColumnName(nameof(Money.Currency))
                     .HasColumnType("VARCHAR(10)")
                     .IsRequired();
            });
        }
    }
}