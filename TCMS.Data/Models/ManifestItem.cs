using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.enums;

namespace TCMS.Data.Models
{
    
    public class ManifestItem
    {
        public int ManifestItemId { get; set; }
        public int ManifestId { get; set; }
        public virtual Manifest Manifest { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
        public ItemStatus Status { get; set; }

        public decimal Price => Product.Price;

        public decimal TotalPrice => Quantity * (Product.Price);
    }
}
