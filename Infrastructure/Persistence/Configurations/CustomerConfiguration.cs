﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasIndex(c => new { c.FirstName, c.LastName })
                .IsUnique();
            
            builder.Property(c => c.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.FullName)
                .HasComputedColumnSql("[LastName] + ' ' + [FirstName]");

            builder.Property(c => c.Address)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.City)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.Country)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.Phone)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.CreditCardNumber)
                .HasMaxLength(16)
                .IsRequired();

            builder.Property(c => c.CardExpMonth)
                .HasMaxLength(2)
                .IsRequired();

            builder.Property(c => c.CardExpYear)
                .HasMaxLength(4)
                .IsRequired();

            builder.HasMany(c => c.Orders)
                .WithOne(o => o.Customer);

            builder.HasOne(c => c.ShoppingCart)
                .WithOne(s => s.Customer)
                .HasForeignKey<ShoppingCart>(c => c.CustomerId);
        }
    }
}