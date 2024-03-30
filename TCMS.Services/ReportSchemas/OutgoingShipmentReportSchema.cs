using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Services.ReportSchemas
{
    public static class OutgoingShipmentReportSchema
    {
        public static readonly ReportSchema Schema = new ReportSchema()
        {
            Filename = "OutgoingShipmentReport.csv",
            Fields = new List<string>
            {
                "ShipmentId",
                "DepartureDate",
                "Destination",
                "TotalCost",
                "PaymentStatus"
            }
        };
    }
}
