using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Data.Models
{
    // Manifest class that stores all the ManifestItems and a method to calculate the total price
    public class Manifest
    {
        public int ManifestId { get; set; }
        public virtual ICollection<ManifestItem>? ManifestItems { get; set; }

        public decimal TotalPrice()
        {
            return ManifestItems.Sum(item => item.TotalPrice);
        }
    }
}
