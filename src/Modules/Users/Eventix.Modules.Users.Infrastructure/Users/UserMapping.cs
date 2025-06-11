using Eventix.Modules.Users.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Modules.Users.Infrastructure.Users
{
    internal sealed class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Address)
                    .HasColumnName("Email")
                    .HasColumnType("varchar")
                    .IsRequired()
                    .HasMaxLength(160);

                email.HasIndex(e => e.Address)
                    .IsUnique()
                    .HasDatabaseName("IX_Users_Email");
            });

            builder.OwnsOne(u => u.Name, name =>
            {
                name.Property(n => n.FirstName)
                    .HasColumnName("FirstName")
                    .HasColumnType("varchar")
                    .IsRequired()
                    .HasMaxLength(50);

                name.Property(n => n.LastName)
                    .HasColumnName("LastName")
                    .HasColumnType("varchar")
                    .IsRequired()
                    .HasMaxLength(50);
            });

            builder.OwnsOne(u => u.AuditInfo, auditInfo =>
            {
                auditInfo.Property(a => a.CreatedAtUtc)
                    .HasColumnName("CreatedAtUtc")
                    .IsRequired();

                auditInfo.Property(a => a.DeletedAtUtc)
                    .HasColumnName("DeletedAtUtc")
                    .IsRequired(false);
            });
        }
    }
}