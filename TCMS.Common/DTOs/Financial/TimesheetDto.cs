using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Financial
{
    public class TimesheetDto
    {
        public int TimeSheetId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime ClockInTime { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? ClockOutTime { get; set; }

        // Calculated property, does not need to be filled when creating a timesheet
        [Display(Name = "Hours Worked")]
        public decimal HoursWorked
        {
            get
            {
                return ClockOutTime.HasValue ? (decimal)(ClockOutTime.Value - ClockInTime).TotalHours : 0;
            }
        }
    }
}
