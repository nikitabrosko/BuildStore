using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasMany(s => s.Products);

            builder.HasMany(s => s.Products)
                .WithMany(p => p.ShoppingCarts);

            builder.HasOne(s => s.User)
                .WithOne(u => u.ShoppingCart)
                .HasForeignKey<ShoppingCart>(s => s.UserId);
        }
    }
}