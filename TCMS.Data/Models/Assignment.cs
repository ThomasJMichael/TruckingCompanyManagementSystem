using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Data.Models
{
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; }
        public string Details { get; set; }

        public string DriverId { get; set; }
        public virtual Driver Driver { get; set; }

        public int ShipmentId { get; set; }
        public Shipment Shipment { get; set; }
    }

}
