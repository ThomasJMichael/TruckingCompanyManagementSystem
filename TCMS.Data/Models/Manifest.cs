using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Data.Models
{
    // Manifest class that stores all the ManifestItems and a method to calculate the total price
    public class Manifest
    {
        public int ManifestId { get; set; }
        public virtual ICollection<ManifestItem> ManifestItems { get; set; }

        public int? ShipmentId { get; set; }
        public virtual Shipment? Shipment { get; set; }

        public int? PurchaseOrderId { get; set; }
        public virtual PurchaseOrder? PurchaseOrder { get; set; }

        public decimal TotalPrice()
        {
            return ManifestItems.Sum(item => item.TotalPrice);
        }
    }
}
