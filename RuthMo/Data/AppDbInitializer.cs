using Microsoft.AspNetCore.Identity;
using RuthMo.Models;

namespace RuthMo.Data;

public class AppDbInitializer
{
    public static async Task SeedRoleToDb(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(RuthMoUserRoles.Admin.ToString()))
                await roleManager.CreateAsync(new IdentityRole(RuthMoUserRoles.Admin.ToString()));

            if (!await roleManager.RoleExistsAsync(RuthMoUserRoles.User.ToString()))
                await roleManager.CreateAsync(new IdentityRole(RuthMoUserRoles.User.ToString()));
        }
    }
}