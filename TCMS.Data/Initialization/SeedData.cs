﻿using Microsoft.AspNetCore.Identity;
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

            //await DatabaseResetter.ResetDatabaseAsync(context);

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
            if (totalEmployeeCount < 150) // Assuming you want a total of 150 employees, including drivers
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



    }
}

