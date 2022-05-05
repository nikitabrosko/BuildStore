using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CategoryBaseConfiguration : IEntityTypeConfiguration<CategoryBase>
    {
        public void Configure(EntityTypeBuilder<CategoryBase> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.Property(c => c.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.Description)
                .HasMaxLength(500)
                .IsRequired();

            builder.HasMany(c => c.Subcategories)
                .WithOne(c => c.Category);

            builder.HasMany(c => c.Products)
                .WithOne(c => c.Category);
        }
    }
}