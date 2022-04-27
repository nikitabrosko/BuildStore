using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasIndex(s => s.CompanyName)
                .IsUnique();

            builder.Property(s => s.CompanyName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(s => s.Address)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(s => s.City)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(s => s.Country)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(s => s.PhoneNumber)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(s => s.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasMany(s => s.Products)
                .WithOne(p => p.Supplier);
        }
    }
}