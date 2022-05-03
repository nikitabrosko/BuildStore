﻿using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Subcategory> Subcategories { get; }

        DbSet<Category> Categories { get; }

        DbSet<Customer> Customers { get; }

        DbSet<Delivery> Deliveries { get; }

        DbSet<Inventory> Inventories { get; }

        DbSet<Order> Orders { get; }

        DbSet<Payment> Payments { get; }

        DbSet<Product> Products { get; }

        DbSet<Supplier> Suppliers { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}