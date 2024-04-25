using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.enums;

namespace TCMS.GUI.Models
{
    public class Shipment
    {
        public int ShipmentId { get; set; }
        public int PurchaseOrderId { get; set; }
        public int VehicleId { get; set; }

        public DateTime DepartureDateTime { get; set; }
        public DateTime EstimatedArrivalDateTime { get; set; }
        public DateTime? ArrivalDateTime { get; set; }
        public bool IsArrived { get; set; }
        public string DriverId { get; set; }

        public ShipmentDirection ShipmentDirection { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int ManifestId { get; set; }
        public Manifest Manifest { get; set; }
    }
}
