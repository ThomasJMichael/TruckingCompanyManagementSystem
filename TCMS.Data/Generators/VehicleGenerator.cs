using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Data.Models;

namespace TCMS.Data.Generators
{
    public static class VehicleGenerator
    {
        private static readonly string[] vehicleTypes = new string[] { "Truck", "Van", "SUV", "Sedan" };

        public static List<Vehicle> GenerateVehicles(int count)
        {
            var vehicleFaker = new Faker<Vehicle>()
                .RuleFor(v => v.VehicleId, f => f.Random.AlphaNumeric(5)) // Generates a 5-character alphanumeric string
                .RuleFor(v => v.Brand, f => f.Vehicle.Manufacturer())
                .RuleFor(v => v.Model, f => f.Vehicle.Model())
                .RuleFor(v => v.Year, f => f.Date.Past(10).Year) // Vehicles from the last 10 years
                .RuleFor(v => v.Type, f => f.PickRandom(vehicleTypes))

                // Initially, related collections might be empty; they can be populated as needed
                .RuleFor(v => v.Parts, new List<PartDetails>())
                .RuleFor(v => v.MaintenanceRecords, new List<MaintenanceRecord>())
                .RuleFor(v => v.RepairRecords, new List<RepairRecord>())
                .RuleFor(v => v.Shipments, new List<Shipment>());

            return vehicleFaker.Generate(count);
        }
    }
}
