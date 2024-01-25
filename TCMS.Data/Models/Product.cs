using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Data.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string  Description { get; set; }
        public decimal Price { get; set; }
        public decimal ShippingPrice { get; set; }

    }
}
