using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.enums;
using TCMS.Data.Models;

namespace TCMS.GUI.Models
{
    public class PurchaseOrderItem
    {
        public int ItemId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public ItemStatus ItemStatus { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice => Quantity * Price;

    }
}
