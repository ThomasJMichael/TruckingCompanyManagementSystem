using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Data.Models;

namespace TCMS.GUI.Models
{
    public class Employee
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; } = "";
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {MiddleName} {LastName}";
        public string Address { get; set; }
        public string AddressLine2 => $"{City}, {State} {Zip}";
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string? HomePhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        public decimal PayRate { get; set; }
        public DateTime StartDate { get; set; }
        public int YearsWithCompany => (DateTime.Now - StartDate).Days / 365;
        public string? UserRole { get; set; }
        public string? CDLNumber { get; set; }
        public DateTime? CDLExperationDate { get; set; }

    }
}
