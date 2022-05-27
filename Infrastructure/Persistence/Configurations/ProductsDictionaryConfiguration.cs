using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProductsDictionaryConfiguration : IEntityTypeConfiguration<ProductsDictionary>
    {
        public void Configure(EntityTypeBuilder<ProductsDictionary> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Count)
                .IsRequired();

            builder.HasOne(p => p.Product)
                .WithMany(p => p.ProductsDictionaries);

            builder.HasOne(p => p.ShoppingCart)
                .WithMany(s => s.ProductsDictionary);

            builder.HasOne(p => p.Order)
                .WithMany(o => o.ProductsDictionary);
        }
    }
}