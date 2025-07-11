using Eventix.Modules.Attendance.Domain.Events.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Modules.Attendance.Infrastructure.Events.Mappings
{
    internal sealed class InvalidCheckInTicketMapping : IEntityTypeConfiguration<InvalidCheckInTicket>
    {
        public void Configure(EntityTypeBuilder<InvalidCheckInTicket> builder)
        {
            builder.ToTable("InvalidCheckInTickets");

            builder.HasKey(c => new { c.EventId, c.Code });
            builder.Property(c => c.Code).HasColumnType("VARCHAR(256)");

            builder.HasIndex(c => c.Code);
        }
    }
}