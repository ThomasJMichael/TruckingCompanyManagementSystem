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
            Console.WriteLine("Seeding roles...");

            foreach (var roleName in RoleHelpers.GetAllRoles())
            {
                if (roleName == null) continue;

                // Seed Roles to the database if they do not exist already
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (roleResult.Succeeded)
                    {
                        Console.WriteLine($"Successfully created role: {roleName}");
                    }
                    else
                    {
                        Console.WriteLine(
                            $"Error creating role: {roleName}. Errors: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                    }
                }
            }

            // Seed default admin user to the database if it does not exist already
            const string adminUserName = "admin";
            var defaultAdmin = await userManager.FindByNameAsync(adminUserName);
            if (defaultAdmin == null)
            {
                Console.WriteLine("Seeding default admin user...");
                var adminUser = new UserAccount
                {
                    UserName = adminUserName,
                    Email = "admin@admin.com"
                };
                var createUserResult = await userManager.CreateAsync(adminUser, "Admin1!");
                if (createUserResult.Succeeded)
                {
                    var addToRoleResult =
                        await userManager.AddToRoleAsync(adminUser,
                            Role.Admin); // Make sure RoleHelpers.Admin is correctly defined and matches the role name
                    if (addToRoleResult.Succeeded)
                    {
                        Console.WriteLine("Successfully created and assigned role to admin user.");
                    }
                    else
                    {
                        Console.WriteLine(
                            $"Error adding admin user to role. Errors: {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    Console.WriteLine(
                        $"Error creating admin user. Errors: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
