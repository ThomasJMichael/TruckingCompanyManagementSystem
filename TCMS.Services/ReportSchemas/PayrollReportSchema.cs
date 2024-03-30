using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Services.ReportSchemas
{
    public static class PayrollReportSchema
    {
        public static readonly ReportSchema Schema = new ReportSchema
        {
            Filename = "PayrollReport.csv",
            Fields = new List<string>
            {
                "EmployeeId",
                "FirstName",
                "MiddleName",
                "LastName",
                "PayPeriodStart",
                "PayPeriodEnd",
                "HoursWorked",
                "PayRate",
                "GrossPay",
                "TaxDeductions",
                "OtherDeductions",
                "NetPay",
            }
        };
    }
}
