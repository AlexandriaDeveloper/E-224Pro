using Microsoft.AspNetCore.Identity;
using Core.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public static class IdentitySeeder
    {
        public static async Task SeedAdminUserAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Check if any users exist
            if (await userManager.Users.AnyAsync()) return;

            // Ensure the admin role exists
            var adminRole = "Admin";
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // Create the admin user
            var adminUser = new AppUser
            {
                UserName = "admin",
                Email = "admin@example.com"
            };

            var result = await userManager.CreateAsync(adminUser, "fr33tim3#"); // Use a strong password

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, adminRole);
            }
        }
    }
}
