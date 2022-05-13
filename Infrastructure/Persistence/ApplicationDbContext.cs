using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<CategoryBase> Categories => Set<CategoryBase>();

        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
        
        public DbSet<Delivery> Deliveries => Set<Delivery>();

        public DbSet<Inventory> Inventories => Set<Inventory>();

        public DbSet<Order> Orders => Set<Order>();

        public DbSet<Payment> Payments => Set<Payment>();

        public DbSet<Product> Products => Set<Product>();

        public DbSet<Supplier> Suppliers => Set<Supplier>();
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public void TurnIdentityInsertOn()
        {
            Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.ShoppingCarts ON");
        }

        public void TurnIdentityInsertOff()
        {
            Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.ShoppingCarts OFF");
        }
    }
}