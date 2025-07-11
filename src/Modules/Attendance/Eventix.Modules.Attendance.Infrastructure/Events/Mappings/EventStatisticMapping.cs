using Eventix.Modules.Attendance.Domain.Events.Models;
using Eventix.Shared.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Modules.Attendance.Infrastructure.Events.Mappings
{
    internal sealed class EventStatisticMapping : IEntityTypeConfiguration<EventStatistic>
    {
        public void Configure(EntityTypeBuilder<EventStatistic> builder)
        {
            builder.ToTable("EventStatistics");

            builder.HasKey(c => c.EventId);

            builder.OwnsOne(e => e.Location, location =>
            {
                location.Property(a => a.Street)
                    .IsRequired()
                    .HasColumnName(nameof(Location.Street))
                    .HasColumnType("VARCHAR(200)");

                location.Property(a => a.Number)
                    .IsRequired()
                    .HasColumnName(nameof(Location.Number))
                    .HasColumnType("VARCHAR(50)");

                location.Property(a => a.ZipCode)
                    .IsRequired()
                    .HasColumnName(nameof(Location.ZipCode))
                    .HasColumnType("VARCHAR(20)");

                location.Property(a => a.AdditionalInfo)
                    .HasColumnName(nameof(Location.AdditionalInfo))
                    .HasColumnType("VARCHAR(250)");

                location.Property(a => a.Neighborhood)
                    .IsRequired()
                    .HasColumnName(nameof(Location.Neighborhood))
                    .HasColumnType("VARCHAR(100)");

                location.Property(a => a.City)
                    .IsRequired()
                    .HasColumnName(nameof(Location.City))
                    .HasColumnType("VARCHAR(100)");

                location.Property(a => a.State)
                    .IsRequired()
                    .HasColumnName(nameof(Location.State))
                    .HasColumnType("VARCHAR(50)");
            });

            builder.Property(c => c.Title).HasColumnType("VARCHAR(80)");
            builder.Property(c => c.Description).HasColumnType("VARCHAR(256)");

            builder.HasMany(c => c.InvalidCheckInTickets).WithOne().HasForeignKey(c => c.EventId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.DuplicateCheckInTickets).WithOne().HasForeignKey(c => c.EventId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}