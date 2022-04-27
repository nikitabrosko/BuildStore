using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Type)
                .HasConversion(
                    t => t.ToString(), 
                    t => (PaymentType)Enum.Parse(typeof(PaymentType), t))
                .IsRequired();

            builder.Property(p => p.Allowed)
                .IsRequired();
        }
    }
}