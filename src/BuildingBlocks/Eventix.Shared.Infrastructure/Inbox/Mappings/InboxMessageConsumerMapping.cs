using Eventix.Shared.Infrastructure.Inbox.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Shared.Infrastructure.Inbox.Mappings
{
    public sealed class InboxMessageConsumerMapping : IEntityTypeConfiguration<InboxMessageConsumer>
    {
        public void Configure(EntityTypeBuilder<InboxMessageConsumer> builder)
        {
            builder.ToTable("InboxMessageConsumers");

            builder.HasKey(c => new { c.InboxMessageId, c.Name });

            builder.Property(c => c.Name)
                .HasColumnType("VARCHAR(256)")
                .IsRequired();
        }
    }
}