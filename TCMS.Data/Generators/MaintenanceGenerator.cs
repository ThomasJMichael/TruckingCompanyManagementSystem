using Bogus;
using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TCMS.Common.enums;
using TCMS.Data.Data;
using TCMS.Data.Models;
using Vehicle = TCMS.Data.Models.Vehicle;

namespace TCMS.Data.Generators
{
    public class MaintenanceGenerator
    {
        public static async Task GenerateMaintenanceRecordsForVehicles(IEnumerable<Vehicle> vehicles,
            int recordsPerVehicle, TcmsContext context)
        {
            var allMaintenanceRecords = new List<MaintenanceRecord>();

            foreach (var vehicle in vehicles)
            {
                var maintenanceRecordsForThisVehicle = new List<MaintenanceRecord>();

                for (int i = 0; i < recordsPerVehicle; i++)
                {
                    var maintenanceRecord = new MaintenanceRecord
                    {
                        RecordType = new Faker().PickRandom<RecordType>(),
                        Description = new Faker().Commerce.ProductName(),
                        MaintenanceDate = new Faker().Date.Past(2),
                        Cost = new Faker().Finance.Amount(100, 10000),
                        Vehicle = vehicle
                    };
                    maintenanceRecord.VehicleId = vehicle.VehicleId;
                    maintenanceRecordsForThisVehicle.Add(maintenanceRecord);
                    context.MaintenanceRecords.Add(maintenanceRecord);
                }
                allMaintenanceRecords.AddRange(maintenanceRecordsForThisVehicle);
            }
            foreach (var entry in context.ChangeTracker.Entries<MaintenanceRecord>())
            {
                Console.WriteLine($"VehicleId before save: {entry.Entity.VehicleId}, state: {entry.State}");
            }
            foreach (var vehicle in context.ChangeTracker.Entries<Vehicle>())
            {
                Console.WriteLine($"Vehicle ID: {vehicle.Entity.VehicleId}, State: {vehicle.State}");
            }
            await context.SaveChangesAsync();
            foreach (var entry in context.ChangeTracker.Entries<MaintenanceRecord>())
            {
                Console.WriteLine($"VehicleId after save: {entry.Entity.VehicleId}, state: {entry.State}");
            }



            var updatedMaintenanceRecords = await context.MaintenanceRecords.ToListAsync();

            foreach (var record in updatedMaintenanceRecords)
            {
                var partDetails = GeneratePartDetails(new Faker().Random.Int(1, 5));
                foreach (var part in partDetails)
                {
                    part.MaintenanceRecordId = record.MaintenanceRecordId;
                    part.VehicleId = record.VehicleId.Value; // Make sure to handle nullable VehicleId if necessary
                    context.PartDetails.Add(part);
                }
            }

            await context.SaveChangesAsync();
        }



        public static List<PartDetails> GeneratePartDetails(int count)
        {
            var partDetailsFaker = new Faker<PartDetails>()
                .RuleFor(pd => pd.PartName, f => f.Commerce.ProductName())
                .RuleFor(pd => pd.PartNumber, f => f.Commerce.Ean13())
                .RuleFor(pd => pd.Quantity, f => f.Random.Int(1, 10))
                .RuleFor(pd => pd.Price, f => f.Finance.Amount(50, 500))
                .RuleFor(pd => pd.Supplier, f => f.Company.CompanyName())
                .RuleFor(pd => pd.isFromStock, f => f.Random.Bool())
                .Generate(count);

            return partDetailsFaker;
        }
    }
}


