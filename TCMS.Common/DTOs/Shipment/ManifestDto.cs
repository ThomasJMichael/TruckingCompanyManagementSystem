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

        [Required(ErrorMessage = "Purchase Order ID is required.")]
        public int PurchaseOrderId { get; set; }

        // This assumes you have a PurchaseOrderDto or similar. Adjust as needed.
        public PurchaseOrderDto PurchaseOrder { get; set; }

        // You might not include ManifestItems directly if they're managed separately
        // but here's how you could define it if included
        public ICollection<ManifestItemDto> ManifestItems { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice => ManifestItems?.Sum(item => item.TotalPrice) ?? 0m;
    }
}
