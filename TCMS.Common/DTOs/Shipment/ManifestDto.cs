using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.DTOs.Financial;

namespace TCMS.Common.DTOs.Shipment
{
    public class ManifestDto
    {
        public int ManifestId { get; set; }

        public int PurchaseOrderId { get; set; }

        public PurchaseOrderDto PurchaseOrder { get; set; }

        public ICollection<ManifestItemDto> ManifestItems { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice => ManifestItems?.Sum(item => item.TotalPrice) ?? 0m;
    }
}
