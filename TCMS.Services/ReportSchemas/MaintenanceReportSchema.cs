using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Services.ReportSchemas
{
    public static class MaintenanceReportSchema
    {
        public static readonly ReportSchema Schema = new ReportSchema
        {
            Filename = "MaintenanceReport.csv",
            Fields = new List<string>
            {
                "VehicleId",
                "Date",
                "MaintenanceType",
                "Description",
                "Cost",
            }
        };
    }
}
