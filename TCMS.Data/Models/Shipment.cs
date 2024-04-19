using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.enums;

namespace TCMS.Data.Models
{
    
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
        public string EmployeeId { get; set; }
        public virtual Driver Driver { get; set; }
        public int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        public int ManifestId { get; set; }
        public virtual Manifest Manifest { get; set; }
        public int PurchaseOrderId { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public DateTime? DepDateTime { get; set; }
        public DateTime? EstimatedArrivalTime { get; set; }
        public DateTime? ActualArrivalTime { get; set; }
        public bool IsPaid { get; set; }

        public decimal TotalCost
        {
            get
            {
                var totalCost = Manifest
                    .ManifestItems
                    .Sum(i => i.TotalPrice);

                return totalCost;
            }
        }

    }
}
