using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TCMS.Data.Models;

namespace TCMS.Data.Initialization
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<UserAccount> userManager)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var roleName in RoleHelpers.GetAllRoles())
            {
                if (roleName == null) continue;
                /*
                 * Seed Roles to the database if they do not exist already
                 */
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            /*
             * seed default admin user to the database if it does not exist already
             */
            const string adminUserName = "admin";
            var defaultAdmin = await userManager.FindByNameAsync(adminUserName);
            if (defaultAdmin == null) ;
            {
                var adminUser = new UserAccount
                {
                    UserName = adminUserName,
                    Email = "admin@admin.com"
                };
                var result = await userManager.CreateAsync(adminUser, "admin");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, Role.Admin);
                }
            }
        }
    }
}
