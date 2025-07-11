using Eventix.Modules.Attendance.Domain.Events.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Modules.Attendance.Infrastructure.Events.Mappings
{
    internal sealed class DuplicateCheckInTicketMapping : IEntityTypeConfiguration<DuplicateCheckInTicket>
    {
        public void Configure(EntityTypeBuilder<DuplicateCheckInTicket> builder)
        {
            builder.ToTable("DuplicateCheckInTickets");

            builder.HasKey(c => new { c.EventId, c.Code });
            builder.Property(c => c.Code).HasColumnType("VARCHAR(256)");

            builder.HasIndex(c => c.Code);
        }
    }
}