using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TCMS.Data.Models;
using TCMS.Data.Generators; // Adjust namespace based on where your generators are located
using System;
using System.Linq;
using System.Threading.Tasks;
using TCMS.Data.Data;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace TCMS.Data.Initialization
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<UserAccount> userManager)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            await ResetDatabase(serviceProvider);
            Console.WriteLine("Seeding roles...");
            await SeedRolesAsync(roleManager);

            Console.WriteLine("Seeding default admin user...");
            await SeedAdminUserAsync(userManager);

            Console.WriteLine("Seeding products and related data...");
            await SeedProductsAsync(serviceProvider);

            Console.WriteLine("Seeding workforce data...");
            await SeedWorkforceAsync(serviceProvider);

            Console.WriteLine("Seeding incidents and tests...");
            await SeedIncidentsAndTests(serviceProvider);

            Console.WriteLine("Seeding timesheets...");
            await SeedTimeSheets(serviceProvider);

            Console.WriteLine("Seeding employees with user accounts...");
            await SeedEmployeesWithAccountsAsync(serviceProvider);

        }

        private static async Task ResetDatabase(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<TcmsContext>();
            await DatabaseResetter.ResetDatabaseAsync(context);
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
            var context = serviceProvider.GetRequiredService<TcmsContext>(); // Adjust the context type and namespace as needed

            if (!context.Products.Any())
            {
                var faker = new Faker();
                var products = ProductGenerator.GenerateProducts(100); // Generate 100 fake products
                context.Products.AddRange(products);
                await context.SaveChangesAsync();

                // After saving, each product now has a ProductId set by EF Core
                var inventories = products.Select(product => new Inventory
                {
                    ProductId = product.ProductId,
                    QuantityOnHand = faker.Random.Int(0, 100) // Generate a random initial inventory quantity
                }).ToList();

                context.Inventories.AddRange(inventories);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedWorkforceAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<TcmsContext>();

            // Seed Drivers first
            var driverCount = await context.Set<Driver>().CountAsync();
            if (driverCount < 50) // Assuming you want at least 50 drivers
            {
                var newDrivers = WorkforceGenerator.GenerateDrivers(50 - driverCount);
                context.AddRange(newDrivers); // Adds Driver entities, which are also Employees due to inheritance
                await context.SaveChangesAsync();
            }

            // Seed regular Employees next
            // This count now includes both Employees and Drivers due to TPH inheritance
            var totalEmployeeCount = await context.Employees.CountAsync();
            if (totalEmployeeCount < 150) 
            {
                // Generate additional regular Employees, excluding the Drivers already counted
                var newEmployees = WorkforceGenerator.GenerateRegularEmployees(150 - totalEmployeeCount);
                context.Employees.AddRange(newEmployees);
                await context.SaveChangesAsync();
            }
        }



        private static async Task SeedIncidentsAndTests(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<TcmsContext>();

            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var faker = new Faker();

                // Seeding incidents if none exist
                if (!await context.IncidentReports.AnyAsync())
                {
                    var drivers = await context.Drivers.ToListAsync();
                    var reports = IncidentAndTestGenerator.GenerateIncidents(drivers, 200, faker);
                    context.IncidentReports.AddRange(reports);
                    await context.SaveChangesAsync();
                }

                // Generate tests for incidents that require it and do not have one yet
                var incidentsNeedingTests = context.IncidentReports
                    .Where(report => report.Type == IncidentType.Accident &&
                                     (report.IsFatal ||
                                      (report.HasInjuries && report.CitationIssued) ||
                                      (report.HasTowedVehicle && report.CitationIssued)) &&
                                     report.DrugAndAlcoholTest == null)
                    .ToList();

                if (incidentsNeedingTests.Any())
                {
                    var testsForIncidents = IncidentAndTestGenerator.GenerateTestsForIncidents(incidentsNeedingTests);
                    context.DrugAndAlcoholTests.AddRange(testsForIncidents);
                    await context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during incident and test seeding: {e.Message}");
                await transaction.RollbackAsync();
            }
        }

        private static async Task SeedTimeSheets(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<TcmsContext>();

            // Check before fetching all employees
            if (await context.TimeSheets.AnyAsync())
            {
                return; // If any timesheets exist, exit early
            }

            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var employees = await context.Employees.ToListAsync();
                var timesheets = TimeSheetGenerator.GenerateTimeSheetsForEmployees(employees, 22);
                context.TimeSheets.AddRange(timesheets);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during timesheet seeding: {e.Message}");
                await transaction.RollbackAsync();
            }
        }
        private static async Task SeedEmployeesWithAccountsAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<UserAccount>>();
            var context = serviceProvider.GetRequiredService<TcmsContext>();

            var nonDriverRoles = RoleHelpers.GetAllRoles().Where(role => role != Role.Driver).ToList();
            var random = new Random();

            // Generate employees
            var employees = WorkforceGenerator.GenerateRegularEmployees(10);
            var drivers = WorkforceGenerator.GenerateDrivers(5);
            var allWorkforce = employees.Concat(drivers).ToList();

            foreach (var person in allWorkforce)
            {
                var username = $"{person.FirstName}.{person.LastName}".ToLower();
                if (await userManager.FindByNameAsync(username) == null)
                {
                    var email = $"{username}@example.com";
                    var user = new UserAccount
                    {
                        UserName = username,
                        Email = email
                    };

                    var createUserResult = await userManager.CreateAsync(user, "SecurePassword123!");
                    if (createUserResult.Succeeded)
                    {
                        var role = person is Driver ? Role.Driver : nonDriverRoles[random.Next(nonDriverRoles.Count)];
                        await userManager.AddToRoleAsync(user, role);

                        // Now, use the ID from the newly created UserAccount as EmployeeId
                        person.EmployeeId = user.Id;
                        context.Employees.Add(person);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to create user account for {username}: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
                    }
                }
            }

            await context.SaveChangesAsync();
        }

    }
}

