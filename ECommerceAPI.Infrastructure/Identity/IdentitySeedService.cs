using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ECommerceAPI.Infrastructure.Identity;

public static class IdentitySeedService
{
    public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        var adminEmail = configuration["AdminSeed:Email"] ?? "admin@ecommerce.com";
        var adminPassword = configuration["AdminSeed:Password"] ?? "Admin@123456";
        var adminDisplayName = configuration["AdminSeed:DisplayName"] ?? "Admin User";

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin is null)
        {
            var adminUser = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                DisplayName = adminDisplayName
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
