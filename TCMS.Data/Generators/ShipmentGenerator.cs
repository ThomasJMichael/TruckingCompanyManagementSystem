using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.enums;
using TCMS.Data.Data;
using TCMS.Data.Models;

namespace TCMS.Data.Generators
{
    public class ShipmentGenerator
    {
        public static List<Shipment> GenerateShipments(
            List<PurchaseOrder> purchaseOrders,
            List<Driver> drivers,
            List<Vehicle> vehicles,
            TcmsContext context)
        {
            var faker = new Faker();
            var shipments = new List<Shipment>();

            foreach (var purchaseOrder in purchaseOrders)
            {
                var numberOfShipmentsForOrder = faker.Random.Int(1, 5); // Assume 1-5 shipments per order
                var masterManifestItems = new Queue<ManifestItem>(purchaseOrder.Manifest.ManifestItems);
                var itemsPerShipment = masterManifestItems.Count / numberOfShipmentsForOrder;
                var leftoverItems = masterManifestItems.Count % numberOfShipmentsForOrder;

                for (int i = 0; i < numberOfShipmentsForOrder; i++)
                {
                    // Create a new Manifest for each Shipment
                    var manifestForShipment = new Manifest
                    {
                        ManifestItems = new List<ManifestItem>()
                    };

                    var manifestItemsCopy = new Queue<ManifestItem>(masterManifestItems);
                    for (int j = 0; j < itemsPerShipment && manifestItemsCopy.Count > 0; j++)
                    {
                        manifestForShipment.ManifestItems.Add(manifestItemsCopy.Dequeue());
                    }


                    if (i == numberOfShipmentsForOrder - 1 && leftoverItems > 0)
                    {
                        for (int j = 0; j < leftoverItems; j++)
                        {
                            manifestForShipment.ManifestItems.Add(masterManifestItems.Dequeue());
                        }
                    }

                    // Ensure the manifest items are saved to generate IDs
                    context.Manifests.Add(manifestForShipment);
                    context.SaveChanges(); // Save changes to get ManifestId

                    var departureDate = faker.Date.Recent(90); // Shipments departed up to 90 days ago
                    var estimatedArrivalDate =
                        departureDate.AddDays(faker.Random.Int(1,
                            30)); // Estimated arrival within 30 days after departure

                    bool hasArrived = faker.Random.Bool(0.7f); // 70% chance the shipment has arrived
                    DateTime? actualArrivalDate =
                        hasArrived
                            ? (DateTime?)estimatedArrivalDate.AddDays(faker.Random.Int(-2, 2))
                            : null; // Actual arrival within 2 days of the estimated date
                    // Create a Shipment
                    var shipment = new Shipment
                    {
                        // Fill in the details for the shipment here
                        Driver = faker.PickRandom(drivers),
                        Vehicle = faker.PickRandom(vehicles),
                        ManifestId = manifestForShipment.ManifestId,
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
                    context.SaveChanges();

                }
            }

            return shipments;
        }
    }
}
