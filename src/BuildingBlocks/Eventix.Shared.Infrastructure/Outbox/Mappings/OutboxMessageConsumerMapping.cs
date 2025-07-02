using Eventix.Shared.Infrastructure.Outbox.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Shared.Infrastructure.Outbox.Mappings
{
    public sealed class OutboxMessageConsumerMapping : IEntityTypeConfiguration<OutboxMessageConsumer>
    {
        public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
        {
            builder.ToTable("OutboxMessageConsumers");

            builder.HasKey(c => new { c.OutboxMessageId, c.Name });

            builder.Property(c => c.Name)
                .HasColumnType("VARCHAR(256)")
                .IsRequired();
        }
    }
}