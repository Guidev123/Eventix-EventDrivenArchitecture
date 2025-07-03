using Eventix.Modules.Attendance.Domain.Attendees.Entities;
using Eventix.Modules.Attendance.Domain.Events.Entities;
using Eventix.Modules.Attendance.Domain.Tickets.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Modules.Attendance.Infrastructure.Tickets.Mappings
{
    internal sealed class TicketMapping : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Code).HasColumnType("VARCHAR(30)");

            builder.HasIndex(t => t.Code).IsUnique();

            builder.HasOne<Attendee>().WithMany().HasForeignKey(t => t.AttendeeId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Event>().WithMany().HasForeignKey(t => t.EventId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}