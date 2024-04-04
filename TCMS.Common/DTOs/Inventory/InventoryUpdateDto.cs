using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Inventory
{
    public class InventoryUpdateDto
    {
        public int ProductId { get; set; }
        public int QuantityOnHand { get; set; }
    }
}
