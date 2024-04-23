using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.DataAnnotations;
using TCMS.Common.enums;
//using TCMS.Data.Models;

namespace TCMS.GUI.Models
{
    public class DrugTest
    {
        public int DrugAndAlcoholTestId { get; set; }

        public string EmployeeId { get; set; }

        public DateTime TestDate { get; set; }
        public TestType TestType { get; set; }
        public TestResult TestResult { get; set; }
        public string TestDetails { get; set; } // Any additional details about the test

        // Potential reference to an incident if the test is post-accident
        public int? IncidentReportId { get; set; }


        // Foll-up details, if applicable
        public DateTime? FollowUpTestDate { get; set; } //Next scheduled test date
        public bool IsFollowUpComplete { get; set; } // Indicates if the follow up has been complete
    }
}
