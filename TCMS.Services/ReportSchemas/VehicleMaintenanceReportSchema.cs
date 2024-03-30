using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Constraints;

namespace TCMS.Services.ReportSchemas
{
    public static class VehicleMaintenanceReportSchema
    {
        public static readonly ReportSchema Schema = new ReportSchema
        {
            Filename = "VehicleMaintenanceReport.csv",
            Fields = new List<string>
            {
                "VehicleId",
                "MaintenanceDate",
                "Description",
                "Cost",
            }
        };
    }
}
