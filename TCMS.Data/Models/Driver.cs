using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Data.Models
{
    public class Driver : Employee
    {
        public string CDLNumber { get; set; }
        public string CDLExperationDate { get; set; }

        // Navigation Property for Assignments
        public ICollection<Assignment>? Assignments { get; set; }
        public ICollection<Shipment> Shipments { get; set; }

        // Navigation properties for Records and Tests
        public ICollection<IncidentReport>? IncidentReports { get; set; }
        public ICollection<DrugAndAlcoholTest>? DrugAndAlcoholTests { get; set; }
    }
}
