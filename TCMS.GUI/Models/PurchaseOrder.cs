using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.enums;

namespace TCMS.GUI.Models
{
    public class PurchaseOrder
    {
        public int PurchaseOrderId { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public ShipmentDirection ShipmentDirection { get; set; }
        public DateTime DateCreated { get; set; }
        public int ManifestId { get; set; }
        public Manifest Manifest { get; set; }
        public ObservableCollection<Shipment> Shipments { get; set; }
        public decimal ShippingCost { get; set; }
        public bool ShippingPaid { get; set; }
        public decimal TotalItemCost => Manifest.Items.Sum(item => item.Price * item.Quantity);
        public decimal TotalCost => TotalItemCost + ShippingCost;

    }
}
