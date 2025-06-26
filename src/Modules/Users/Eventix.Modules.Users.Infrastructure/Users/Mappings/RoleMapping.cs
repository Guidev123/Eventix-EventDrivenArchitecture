using Eventix.Modules.Users.Domain.Users.Entities;
using Eventix.Modules.Users.Domain.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Modules.Users.Infrastructure.Users.Mappings
{
    internal sealed class RoleMapping : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(r => r.Name);

            builder.Property(r => r.Name).HasColumnType("VARCHAR(50)").IsRequired();

            builder
                .HasMany<User>()
                .WithMany(u => u.Roles)
                .UsingEntity(joinBuilder =>
                {
                    joinBuilder.ToTable("UserRoles");

                    joinBuilder.Property("RolesName").HasColumnName("RoleName");
                });

            builder.HasData(
                Role.Member,
                Role.Administrator);
        }
    }
}