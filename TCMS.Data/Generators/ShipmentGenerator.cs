using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.enums;
using TCMS.Data.Models;

namespace TCMS.Data.Generators
{
    public class ShipmentGenerator
    {
        public static List<Shipment> GenerateShipments( List<PurchaseOrder> purchaseOrders, List<Driver> drivers, List<Vehicle> vehicles)
        {
            var faker = new Faker();
            var shipments = new List<Shipment>();

            foreach (var purchaseOrder in purchaseOrders)
            {
                // Decide how many shipments are needed for this purchase order
                int numberOfShipmentsForOrder = faker.Random.Int(1, 5); // For example, between 1 and 5 shipments per order

                var manifestItems = purchaseOrder.Manifest.ManifestItems.ToList();

                for (int i = 0; i < numberOfShipmentsForOrder; i++)
                {
                    // Here, divide manifestItems among the shipments
                    var itemsForThisShipment = manifestItems
                        .Skip(i * manifestItems.Count / numberOfShipmentsForOrder)
                        .Take(manifestItems.Count / numberOfShipmentsForOrder)
                        .ToList();

                    // Create a new Manifest for this Shipment
                    var manifestForShipment = new Manifest
                    {
                        ManifestItems = itemsForThisShipment
                    };
                    var departureDate = faker.Date.Recent(90); // Shipments departed up to 90 days ago
                    var estimatedArrivalDate = departureDate.AddDays(faker.Random.Int(1, 30)); // Estimated arrival within 30 days after departure

                    bool hasArrived = faker.Random.Bool(0.7f); // 70% chance the shipment has arrived
                    DateTime? actualArrivalDate = hasArrived ? (DateTime?)estimatedArrivalDate.AddDays(faker.Random.Int(-2, 2)) : null; // Actual arrival within 2 days of the estimated date
                    // Create a Shipment
                    var shipment = new Shipment
                    {
                        // Fill in the details for the shipment here
                        Driver = faker.PickRandom(drivers),
                        Vehicle = faker.PickRandom(vehicles),
                        Manifest = manifestForShipment,
                        PurchaseOrder = purchaseOrder,
                        Direction = faker.PickRandom<ShipmentDirection>(),
                        Company = purchaseOrder.Company,
                        Address = purchaseOrder.Address,
                        City = purchaseOrder.City,
                        State = purchaseOrder.State,
                        Zip = purchaseOrder.Zip,
                        hasArrived = hasArrived,
                        DepDateTime = departureDate,
                        EstimatedArrivalTime = estimatedArrivalDate,
                        ActualArrivalTime = actualArrivalDate,
                        IsPaid = faker.Random.Bool(),
                    };

                    shipment.EmployeeId = shipment.Driver.EmployeeId;
                    shipment.VehicleId = shipment.Vehicle.VehicleId;
                    shipment.PurchaseOrderId = purchaseOrder.PurchaseOrderId;

                    shipments.Add(shipment);
                }
            }

            return shipments;
        }
    }



}
