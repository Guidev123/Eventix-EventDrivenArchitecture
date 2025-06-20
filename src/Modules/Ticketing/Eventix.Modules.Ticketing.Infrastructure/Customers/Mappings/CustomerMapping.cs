using Eventix.Modules.Ticketing.Domain.Customers.Entities;
using Eventix.Shared.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventix.Modules.Ticketing.Infrastructure.Customers.Mappings
{
    internal sealed class CustomerMapping : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);

            builder.OwnsOne(c => c.Name, name =>
            {
                name.Property(c => c.FirstName).HasColumnName("FirstName").HasColumnType("VARCHAR(50)");
                name.Property(c => c.LastName).HasColumnName("LastName").HasColumnType("VARCHAR(50)");
            });

            builder.OwnsOne(c => c.Email, email =>
            {
                email.Property(c => c.Address).HasColumnName(nameof(Email)).HasColumnType("VARCHAR(160)");
                email.HasIndex(c => c.Address).IsUnique();
            });
        }
    }
}