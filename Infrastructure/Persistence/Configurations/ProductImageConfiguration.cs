using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Product)
                .WithMany(p => p.Images);

            builder.Ignore(c => c.PictureRaw);

            builder.Property(p => p.Picture)
                .IsRequired();
        }
    }
}