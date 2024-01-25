using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Data.Models
{
    public class PurchaseOrder
    {
        public int PurchaseOrderId { get; set; }
        public DateTime DateCreated { get; set; }
        public string OrderNumber { get; set; }

        public virtual ICollection<Manifest> Manifests { get; set; }
    }
}
