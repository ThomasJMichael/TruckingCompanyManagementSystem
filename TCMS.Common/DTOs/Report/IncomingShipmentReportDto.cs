using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Report
{
    public class IncomingShipmentReportDto
    {
        public int ShipmentId { get; set; }
        public DateTime? ActualArrivalTime { get; set; }
        public string? Company { get; set; }
        public decimal TotalCost { get; set; }
        public bool IsFullyPaid { get; set; }
    }

}
