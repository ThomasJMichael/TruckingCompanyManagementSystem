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
using TCMS.Common.Enums;
using Bogus.DataSets;
using TCMS.Common.enums;

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

            Console.WriteLine("Seeding default user...");
            await SeedDefaultUser(userManager);

            Console.WriteLine("Seeding vehicle data...");
            await SeedVehicleData(serviceProvider);

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

            Console.WriteLine("Seeding vehicle related data...");
            await SeedVehicleRelatedData(serviceProvider);

            Console.WriteLine("Seeding shipping data...");
            await SeedShippingData(serviceProvider);

            Console.Write("Seed driver user...");
            await SeedDriverUserAccount(userManager, serviceProvider.GetRequiredService<TcmsContext>());

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

        private static async Task SeedDefaultUser(UserManager<UserAccount> userManager)
        {
            const string defaultUserName = "default";
            var defaultAdmin = await userManager.FindByNameAsync(defaultUserName);
            if (defaultAdmin == null)
            {
                var adminUser = new UserAccount { UserName = defaultUserName, Email = "default@default.com" };
                await userManager.CreateAsync(adminUser, "Admin1!");
                await userManager.AddToRoleAsync(adminUser, Role.Default);
            }
        }

        private static async Task SeedDriverUserAccount(UserManager<UserAccount> userManager, TcmsContext context)
        {
            // Create default driver user details
            const string defaultDriverUserName = "driver";
            const string driverEmail = "driver@tcms.com";
            const string driverPassword = "Admin1!";

            // Check if the driver user already exists
            var driverUser = await userManager.FindByNameAsync(defaultDriverUserName);
            if (driverUser == null)
            {
                // Create and add new driver user
                driverUser = new UserAccount { UserName = defaultDriverUserName, Email = driverEmail };
                var result = await userManager.CreateAsync(driverUser, driverPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(driverUser, Role.Driver);

                    // Assign the user account to an existing driver who has a shipment
                    // Make sure there is at least one shipment with a driver assigned
                    var shipmentWithDriver = context.Shipments
                        .Include(s => s.Driver)
                        .FirstOrDefault(s => s.Driver != null);

                    if (shipmentWithDriver != null)
                    {
                        // Get the driver entity from the shipment
                        var driver = shipmentWithDriver.Driver;

                        // Link the user account with the driver entity
                        driver.UserAccountId = driverUser.Id;
                        context.Drivers.Update(driver);
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    throw new Exception("Failed to create driver user account");
                }
            }
        }


        private static async Task SeedProductsAsync(IServiceProvider serviceProvider)
        {
            var context =
                serviceProvider.GetRequiredService<TcmsContext>(); // Adjust the context type and namespace as needed

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
                    var vehicles = await context.Vehicles.ToListAsync();
                    var reports = IncidentAndTestGenerator.GenerateIncidents(drivers, vehicles, 200, faker);
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
                        Console.WriteLine(
                            $"Failed to create user account for {username}: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
                    }
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedVehicleData(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<TcmsContext>();

            if (!context.Vehicles.Any())
            {
                var vehicles = VehicleGenerator.GenerateVehicles(50);
                context.Vehicles.AddRange(vehicles);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedVehicleRelatedData(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<TcmsContext>();

            // Fetch all vehicles from the database, including any existing Parts and MaintenanceRecords.
            var vehicles = await context.Vehicles
                .Include(v => v.Parts)
                .Include(v => v.MaintenanceRecords)
                .ToListAsync();

            foreach (var vehicle in vehicles)
            {
                // Check if the vehicle already has maintenance records, if not, generate them
                if (!vehicle.MaintenanceRecords.Any())
                {
                    var maintenanceRecords =
                        MaintenanceGenerator
                            .GenerateMaintenanceRecordsForVehicle(vehicle,
                                5); // Generate 5 maintenance records per vehicle
                    context.MaintenanceRecords.AddRange(maintenanceRecords);
                }

                // Check if the vehicle already has parts, if not, generate them
                if (!vehicle.Parts.Any())
                {
                    // Since MaintenanceRecords are now associated with Parts, ensure that Parts are generated after MaintenanceRecords
                    var partsForVehicle =
                        MaintenanceGenerator.GeneratePartDetails(20); // Generate 20 parts for each vehicle
                    foreach (var part in partsForVehicle)
                    {
                        part.VehicleId = vehicle.VehicleId; // Associate part with the vehicle
                        // Optionally associate parts with a randomly selected maintenance record
                        part.MaintenanceRecordId = vehicle.MaintenanceRecords.Any()
                            ? new Faker().PickRandom(vehicle.MaintenanceRecords).MaintenanceRecordId
                            : (int?)null;
                    }

                    context.PartDetails.AddRange(partsForVehicle);
                }
            }

            // Save all changes to the database.
            await context.SaveChangesAsync();
        }

        public static async Task SeedShippingData(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<TcmsContext>();

            if (context.PurchaseOrders.ToList().Count > 10)
            {
                return;
            }

            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                Console.WriteLine("Seeding manifest data...");
                var manifests = await SeedManifestsAsync(context);

                Console.WriteLine("Seeding purchase orders...");
                var purchaseOrders = await SeedPurchaseOrdersAsync(context);

                Console.WriteLine("Seeding shipment data...");
                await SeedShipmentsAsync(context);

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during shipping data seeding: {e.Message}");
                await transaction.RollbackAsync();
            }
        }

        private static async Task<List<Manifest>> SeedManifestsAsync(TcmsContext context)
        {
            var manifests = ManifestGenerator.GenerateManifests(50);

            await context.Manifests.AddRangeAsync(manifests);
            await context.SaveChangesAsync();

            var products = context.Products.ToList();
            foreach (var manifest in manifests)
            {
                var manifestItems = ManifestItemGenerator.GenerateManifestItemsForManifest(manifest.ManifestId, 10, products); // Generate 10 ManifestItems per Manifest
                manifest.ManifestItems = manifestItems;
            }

            // Save ManifestItems to the database
            foreach (var manifest in manifests)
            {
                await context.ManifestItems.AddRangeAsync(manifest.ManifestItems);
            }
            await context.SaveChangesAsync();

            return manifests;
        }


        private static async Task<List<PurchaseOrder>> SeedPurchaseOrdersAsync(TcmsContext context)
        {
            // Link PurchaseOrders with existing Manifests
            var manifests = await context.Manifests.ToListAsync();
            var purchaseOrders = PurchaseOrderGenerator.GeneratePurchaseOrders(50, manifests);
            await context.PurchaseOrders.AddRangeAsync(purchaseOrders);
            await context.SaveChangesAsync();
            return purchaseOrders;
        }

        private static async Task SeedShipmentsAsync(TcmsContext context)
        {
            // Fetch all PurchaseOrders without manifests
            var purchaseOrders = await context.PurchaseOrders.ToListAsync();

            // Fetch all Manifests and reassociate them manually
            var manifestIds = purchaseOrders.Select(po => po.ManifestId).ToList();
            var manifests = await context.Manifests
                .Where(m => manifestIds.Contains(m.ManifestId))
                .Include(m => m.ManifestItems)
                .ToListAsync();

            // Create a dictionary to quickly find manifests by ID
            var manifestDictionary = manifests.ToDictionary(m => m.ManifestId);


            // Associate manifests with purchase orders manually
            foreach (var po in purchaseOrders)
            {
                if (manifestDictionary.TryGetValue(po.ManifestId, out var manifest))
                {
                    po.Manifest = manifest;
                }
            }

            var drivers = context.Drivers.ToList();
            var vehicles = context.Vehicles.ToList();

            // Generate Shipments with references to PurchaseOrders
            var shipments = ShipmentGenerator.GenerateShipments(purchaseOrders, drivers, vehicles);

            // Save new manifests created for Shipments
            foreach (var shipment in shipments)
            {
                context.Manifests.Add(shipment.Manifest);
            }
            await context.SaveChangesAsync();

            // Now we can save the Shipments, as each Manifest has an ID
            await context.Shipments.AddRangeAsync(shipments);
            await context.SaveChangesAsync();
        }
    }
}

