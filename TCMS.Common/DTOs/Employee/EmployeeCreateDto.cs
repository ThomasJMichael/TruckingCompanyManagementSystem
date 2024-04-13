using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Employee
{
    public class EmployeeCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string? HomePhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        public decimal PayRate { get; set; }
        public DateTime StartDate { get; set; }
    }

}
