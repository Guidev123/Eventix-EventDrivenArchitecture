using Eventix.Modules.Attendance.Domain.Events.Entities;
using Eventix.Modules.Attendance.Domain.Events.ValueObjects;
using Eventix.Shared.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Modules.Attendance.Infrastructure.Events.Mappings
{
    internal sealed class EventMapping : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");

            builder.HasKey(e => e.Id);

            builder.OwnsOne(e => e.Specification, spec =>
            {
                spec.Property(s => s.Title).HasColumnName(nameof(EventSpecification.Title)).HasColumnType("VARCHAR(80)").IsRequired();
                spec.Property(s => s.Description).HasColumnName(nameof(EventSpecification.Description)).HasColumnType("VARCHAR(256)").IsRequired();
            });

            builder.OwnsOne(e => e.DateRange, dateRange =>
            {
                dateRange.Property(d => d.StartsAtUtc).HasColumnName(nameof(DateRange.StartsAtUtc)).IsRequired();
                dateRange.Property(d => d.EndsAtUtc).HasColumnName(nameof(DateRange.EndsAtUtc)).IsRequired(false);
            });

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
        }
    }
}