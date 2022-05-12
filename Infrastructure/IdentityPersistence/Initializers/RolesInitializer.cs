using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.IdentityPersistence.Initializers
{
    public static class RolesInitializer
    {
        public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("admin") is null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await roleManager.FindByNameAsync("user") is null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
        }
    }
}