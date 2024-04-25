using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Shipment
{
    public class ShipmentManifestDto
    {
        public int ManifestId { get; set; }
        public int ShipmentId { get; set; }
        public ICollection<ManifestItemDto> ManifestItems { get; set; }
        public decimal TotalPrice => ManifestItems?.Sum(item => item.TotalPrice) ?? 0m;
    }

}
