using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TCMS.Data.Models;
using TCMS.Data.Generators; // Adjust namespace based on where your generators are located
using System;
using System.Linq;
using System.Threading.Tasks;
using TCMS.Data.Data;

namespace TCMS.Data.Initialization
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<UserAccount> userManager)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            Console.WriteLine("Seeding roles...");
            await SeedRolesAsync(roleManager);

            Console.WriteLine("Seeding default admin user...");
            await SeedAdminUserAsync(userManager);

            Console.WriteLine("Seeding products...");
            await SeedProductsAsync(serviceProvider);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in RoleHelpers.GetAllRoles())
            {
                if (roleName == null) continue;

                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<UserAccount> userManager)
        {
            const string adminUserName = "admin";
            var defaultAdmin = await userManager.FindByNameAsync(adminUserName);
            if (defaultAdmin == null)
            {
                var adminUser = new UserAccount { UserName = adminUserName, Email = "admin@admin.com" };
                await userManager.CreateAsync(adminUser, "Admin1!");
                await userManager.AddToRoleAsync(adminUser, Role.Admin); 
            }
        }

        private static async Task SeedProductsAsync(IServiceProvider serviceProvider)
        {
            // Use the context service directly to avoid DI scope issues
            var context = serviceProvider.GetRequiredService<TcmsContext>(); // Adjust the context type and namespace

            if (!context.Products.Any())
            {
                var products = ProductGenerator.GenerateProducts(100); // Generate 100 fake products
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}

