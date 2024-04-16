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
        public DateTime DateAdded { get; set; }
        public DateTime? DateRemoved { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateShipped { get; set; }
        public DateTime? DateReturned { get; set; }
        public DateTime? DateCancelled { get; set; }
        public DateTime? DateRefunded { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? DatePaid { get; set; }
        public DateTime? DateInvoiced { get; set; }
        public DateTime? DatePaidFor { get; set; }
        public DateTime? DatePaidInFull { get; set; }
        public DateTime? DatePaidInFullAndReceived { get; set; }
        public DateTime? DatePaidInFullAndShipped { get; set; }

        public decimal Price => Product.Price;

        public decimal TotalPrice => Quantity * (Product.Price);
    }
}
