using Eventix.Modules.Users.Domain.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Modules.Users.Infrastructure.Users.Mappings
{
    internal sealed class PermissionMapping : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");

            builder.HasKey(p => p.Code);

            builder.Property(p => p.Code).HasColumnType("VARCHAR(100)").IsRequired();

            builder
                .HasMany<Role>()
                .WithMany()
                .UsingEntity(joinBuilder =>
                {
                    joinBuilder.ToTable("RolePermissions");
                });
        }
    }
}