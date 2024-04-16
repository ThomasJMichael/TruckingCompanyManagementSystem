using Bogus;
using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.enums;
using TCMS.Data.Models;
using Vehicle = TCMS.Data.Models.Vehicle;

namespace TCMS.Data.Generators
{
    public class MaintenanceGenerator
    {
        public static List<MaintenanceRecord> GenerateMaintenanceRecordsForVehicle(Vehicle vehicle, int count)
        {
            var maintenanceRecords = new Faker<MaintenanceRecord>()
                .RuleFor(mr => mr.RecordType, f => f.PickRandom<RecordType>())
                .RuleFor(mr => mr.Description, f => f.Commerce.ProductName())
                .RuleFor(mr => mr.MaintenanceDate, f => f.Date.Past(2))
                .RuleFor(mr => mr.Cost, f => f.Finance.Amount(100, 10000))
                .RuleFor(mr => mr.VehicleId, vehicle.VehicleId)
                .Generate(count);

            foreach (var record in maintenanceRecords)
            {
                var faker = new Faker();
                // Assume each maintenance record may involve between 1 and 5 parts.
                var partDetails = GeneratePartDetails(faker.Random.Int(1, 5));

                // Associate the generated PartDetails with the MaintenanceRecord
                record.PartDetails = partDetails;

                // Also associate the generated PartDetails with the Vehicle
                foreach (var part in partDetails)
                {
                    part.VehicleId = vehicle.VehicleId;
                    vehicle.Parts.Add(part); // Assuming the Vehicle.Parts collection is initialized.
                }
            }

            return maintenanceRecords;

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


