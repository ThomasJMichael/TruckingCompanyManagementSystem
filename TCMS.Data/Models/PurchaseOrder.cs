using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.enums;

namespace TCMS.Data.Models
{
    public class PurchaseOrder
    {
        public int PurchaseOrderId { get; set; }
        public ShipmentDirection Direction { get; set; }
        public DateTime DateCreated { get; set; }
        public int ManifestId { get; set; }
        public virtual Manifest Manifest { get; set; }

        public virtual ICollection<Shipment>? Shipments { get; set; }


    }
}
