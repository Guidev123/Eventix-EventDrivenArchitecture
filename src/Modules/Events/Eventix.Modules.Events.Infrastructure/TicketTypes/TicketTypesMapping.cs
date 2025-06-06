using Eventix.Modules.Events.Domain.TicketTypes.Entities;
using Eventix.Modules.Events.Domain.TicketTypes.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Modules.Events.Infrastructure.TicketTypes
{
    public sealed class TicketTypesMapping : IEntityTypeConfiguration<TicketType>
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
            });

            builder.OwnsOne(t => t.Price, price =>
            {
                price.Property(p => p.Amount)
                     .HasColumnName(nameof(Money.Amount))
                     .HasColumnType("MONEY");

                price.Property(p => p.Currency)
                     .HasColumnName(nameof(Money.Currency))
                     .HasColumnType("VARCHAR(10)")
                     .IsRequired();
            });
        }
    }
}