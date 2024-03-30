using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Data.Models
{
    public enum ShipmentDirection
    {
        Inbound,
        Outbound
    }
    public class Shipment
    {
        public int ShipmentId { get; set; }

        public ShipmentDirection Direction { get; set; }

        public bool hasArrived { get; set; }

        public string Company { get; set; }
        public string Address { get; set; }
        public string City { get; set;}
        public string State { get; set; }
        public string Zip { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public string VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public decimal ShippingCost { get; set; }
        public bool IsShippingCostPaid { get; set; }
        public DateTime? ShippingCostPaymentDate { get; set; }

        public int ManifestId { get; set; }
        public virtual Manifest Manifest { get; set; }
        public int PurchaseOrderId { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public DateTime? DepDateTime { get; set; }
        public DateTime? EstimatedArrivalTime { get; set; }
        public DateTime? ActualArrivalTime { get; set; }

        public decimal TotalCost
        {
            get
            {
                var totalCost = Manifest
                    .ManifestItems
                    .Sum(i => i.TotalPrice);

                return totalCost + ShippingCost;
            }
        }

        public bool isFullyPaid
        {
            get
            {
                var areAllItemsPaid = Manifest
                    .ManifestItems
                    .All(i => i.IsPaid);

                return areAllItemsPaid && IsShippingCostPaid;
            }
        }
    }
}
