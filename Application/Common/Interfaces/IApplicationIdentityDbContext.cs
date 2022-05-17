using System.Threading;
using System.Threading.Tasks;
using Domain.IdentityEntities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationIdentityDbContext
    {
        DbSet<User> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}