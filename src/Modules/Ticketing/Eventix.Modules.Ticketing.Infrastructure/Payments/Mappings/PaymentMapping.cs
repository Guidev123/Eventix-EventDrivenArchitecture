using Eventix.Modules.Ticketing.Domain.Orders.Entities;
using Eventix.Modules.Ticketing.Domain.Payments.Entities;
using Eventix.Modules.Ticketing.Domain.Payments.ValueObjects;
using Eventix.Shared.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Modules.Ticketing.Infrastructure.Payments.Mappings
{
    internal sealed class PaymentMapping : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);

            builder.OwnsOne(c => c.Amount, amount =>
            {
                amount.Property(c => c.Amount)
                .HasColumnName(nameof(Money.Amount))
                .HasColumnType("MONEY");

                amount.Property(c => c.Currency)
                .HasColumnName("AmountCurrency")
                .HasColumnType("VARCHAR(10)");
            });

            builder.OwnsOne(c => c.AmountRefunded, amount =>
            {
                amount.Property(c => c.Amount)
                .HasColumnName("AmountRefunded")
                .HasColumnType("MONEY");

                amount.Property(c => c.Currency)
                .HasColumnName("AmountRefundedCurrency")
                .HasColumnType("VARCHAR(10)");
            });

            builder.OwnsOne(c => c.DateInfo, dateInfo =>
            {
                dateInfo.Property(c => c.CreatedAtUtc)
                .HasColumnName(nameof(PaymentDateInfo.CreatedAtUtc)).IsRequired();

                dateInfo.Property(c => c.RefundedAtUtc)
                .HasColumnName(nameof(PaymentDateInfo.RefundedAtUtc)).IsRequired(false);
            });

            builder.HasOne<Order>().WithMany().HasForeignKey(p => p.OrderId).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.TransactionId).IsUnique();
        }
    }
}