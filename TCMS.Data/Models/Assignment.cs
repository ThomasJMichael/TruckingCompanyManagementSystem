using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Data.Models
{
    public class Assignment
    {
        public int AssignmentId { get; set; }

        [ForeignKey("Driver")]
        public string EmployeeId { get; set; }
        public virtual Driver Driver { get; set; }

        public int ShipmentId { get; set; }
        public Shipment Shipment { get; set; }
    }

}
