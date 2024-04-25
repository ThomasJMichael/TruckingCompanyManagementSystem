using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.enums;

namespace TCMS.Common.DTOs.Shipment
{
    public class ManifestItemDto
    {
        public int ManifestItemId { get; set; }

        public int ProductId { get; set; }
        public ItemStatus ItemStatus { get; set; }

        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Shipping price must be greater than 0.")]
        public decimal ShippingPrice { get; set; }

        public decimal TotalPrice => Quantity * (Price + ShippingPrice);
    }
}
