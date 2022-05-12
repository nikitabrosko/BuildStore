using Application.Common.Interfaces;
using Infrastructure.IdentityPersistence;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());

            services.AddDbContext<ApplicationIdentityDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("IdentityConnection")));

            services.AddScoped<IApplicationIdentityDbContext>(provider =>
                provider.GetRequiredService<ApplicationIdentityDbContext>());

            return services;
        }
    }
}