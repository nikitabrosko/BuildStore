using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<CategoryBase> Categories { get; }
        
        DbSet<Customer> Customers { get; }

        DbSet<ShoppingCart> ShoppingCarts { get; }

        DbSet<Delivery> Deliveries { get; }

        DbSet<Inventory> Inventories { get; }

        DbSet<Order> Orders { get; }

        DbSet<Payment> Payments { get; }

        DbSet<Product> Products { get; }

        DbSet<Supplier> Suppliers { get; }

        DbSet<ProductsDictionary> ProductsDictionaries { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}