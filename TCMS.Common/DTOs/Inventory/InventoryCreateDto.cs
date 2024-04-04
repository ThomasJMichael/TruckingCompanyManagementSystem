using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Inventory
{
    public class InventoryCreateDto
    {
        public int ProductId { get; set; }
        public int InitialQuantity { get; set; }
    }
}
