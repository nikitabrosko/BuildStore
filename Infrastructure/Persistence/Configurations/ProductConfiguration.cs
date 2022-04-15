using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasIndex(p => p.Name)
                .IsUnique();

            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(p => p.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.QuantityPerUnit)
                .IsRequired();

            builder.HasOne(p => p.SupplierId)
                .WithMany(s => s.Products);

            builder.HasMany(p => p.Categories)
                .WithMany(c => c.Products);

            builder.Property(p => p.Size)
                .HasPrecision(7, 2)
                .IsRequired();

            builder.Property(p => p.Color)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Discount)
                .HasPrecision(7, 2);

            builder.Property(p => p.Weight)
                .HasPrecision(7, 2)
                .IsRequired();

            builder.Property(p => p.Picture)
                .IsRequired();
        }
    }
}