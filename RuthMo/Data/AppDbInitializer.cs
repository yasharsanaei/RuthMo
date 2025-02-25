using Microsoft.AspNetCore.Identity;
using NuGet.Versioning;
using RuthMo.Models;

namespace RuthMo.Data;

public static class AppDbInitializer
{
    public static async Task SeedRoleToDb(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<RuthMoUser>>();

            if (!await roleManager.RoleExistsAsync(RuthMoUserRoles.Admin.ToString()))
                await roleManager.CreateAsync(new IdentityRole(RuthMoUserRoles.Admin.ToString()));

            if (!await roleManager.RoleExistsAsync(RuthMoUserRoles.User.ToString()))
                await roleManager.CreateAsync(new IdentityRole(RuthMoUserRoles.User.ToString()));

            if (await userManager.FindByEmailAsync("admin@ruthmo.com") == null)
            {
                RuthMoUser ruthMoAdmin = new RuthMoUser
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "admin@ruthmo.com",
                    UserName = "admin",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await userManager.CreateAsync(ruthMoAdmin, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(ruthMoAdmin, [RuthMoUserRoles.Admin.ToString()]);
                }
            }
        }
    }
}