using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Services.ReportSchemas
{
    public static class IncomingShipmentReportSchema
    {
        public static readonly ReportSchema Schema = new ReportSchema
        {
            Filename = "IncomingShipmentsReport",
            Fields = new List<string>
            {
                "ShipmentId",
                "ActualArrivalTime",
                "Company",
                "TotalCost",
                "PaymentStatus",
            }
        };
    }
}
