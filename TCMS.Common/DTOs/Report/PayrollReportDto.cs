using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Report
{
    public class PayrollReportDto
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; } = "";
        public string LastName { get; set; }
        public DateTime PayPeriodStart { get; set; }
        public DateTime PayPeriodEnd { get; set; }
        public decimal HoursWorked { get; set; }
        public decimal PayRate { get; set; }
        public decimal GrossPay { get; set; }
        public decimal TaxDeductions { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal NetPay { get; set; }
    }

}
