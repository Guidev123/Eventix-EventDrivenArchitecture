using Eventix.Shared.Infrastructure.Inbox.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Shared.Infrastructure.Inbox.Mappings
{
    public sealed class InboxMessageMapping : IEntityTypeConfiguration<InboxMessage>
    {
        public void Configure(EntityTypeBuilder<InboxMessage> builder)
        {
            builder.ToTable("InboxMessages");

            builder.HasKey(im => im.Id);

            builder.Property(x => x.Type).HasColumnType("VARCHAR(200)");
            builder.Property(x => x.Content).HasColumnType("VARCHAR(3000)");
            builder.Property(x => x.Error).HasColumnType("VARCHAR(256)");
        }
    }
}