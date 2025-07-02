using Eventix.Shared.Infrastructure.Outbox.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Shared.Infrastructure.Outbox.Mappings
{
    public sealed class OutboxMessageMapping : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type).HasColumnType("VARCHAR(200)");
            builder.Property(x => x.Content).HasColumnType("VARCHAR(3000)");
            builder.Property(x => x.Error).HasColumnType("VARCHAR(256)");
        }
    }
}