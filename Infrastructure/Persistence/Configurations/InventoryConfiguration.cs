using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.IsInStock)
                .IsRequired();

            builder.Property(i => i.UnitsInStock)
                .IsRequired();

            builder.Property(i => i.AvailableColors);

            builder.Property(i => i.AvailableSizes);

            builder.HasOne(i => i.ProductId);
        }
    }
}