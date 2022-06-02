using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Application;
using Domain.IdentityEntities;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.IdentityPersistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace WebUI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);

            services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 4;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Lockout.MaxFailedAccessAttempts = 100;
                    options.Lockout.DefaultLockoutTimeSpan = options.Lockout.DefaultLockoutTimeSpan.Add(TimeSpan.FromMinutes(55));
                })
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/IdentityControllers/Account/Login";
                options.AccessDeniedPath = "/Category/Index";
                options.SlidingExpiration = true;
            });

            services.AddHttpContextAccessor();

            services.AddControllersWithViews()
                .AddFluentValidation(x => 
                    x.AutomaticValidationEnabled = false);

            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
