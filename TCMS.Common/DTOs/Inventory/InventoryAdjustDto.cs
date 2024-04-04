using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Inventory
{
    public class InventoryAdjustDto
    {
        public int ProductId { get; set; }
        public int QuantityChange { get; set; } // Positive for increase, negative for decrease
    }
}
