using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Data.Models
{
    public class TimeSheet
    {
        public int TimeSheetId { get; set; }
        public string EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public DateTime ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }
        public DateTime Date { get; set; }
        public decimal HoursWorked => ClockOut.HasValue ? ((ClockOut.Value - ClockIn).Hours + ((ClockOut.Value - ClockIn).Minutes / 60M)) : 0;
    }

}
