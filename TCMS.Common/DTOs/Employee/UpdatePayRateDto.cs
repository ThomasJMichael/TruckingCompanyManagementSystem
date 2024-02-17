using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Employee
{
    public class UpdatePayRateDto
    {
        [Required]
        [StringLength(50)]
        public string EmployeeId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid pay rate")]
        public decimal NewPayRate { get; set; }
    }

}
