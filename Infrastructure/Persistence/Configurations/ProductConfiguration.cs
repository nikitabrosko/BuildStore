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

            builder.HasOne(p => p.Supplier)
                .WithMany(s => s.Products);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products);
            
            builder.Property(p => p.Discount);

            builder.Property(p => p.Weight)
                .IsRequired();

            builder.Ignore(c => c.PictureRaw);

            builder.Property(c => c.Picture)
                .IsRequired();

            builder.HasMany(p => p.ShoppingCarts)
                .WithMany(s => s.Products);

            builder.HasMany(p => p.Orders)
                .WithMany(p => p.Products);
        }
    }
}