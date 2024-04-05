using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Data.Models
{ 
    public enum TestResult
    {
    Negative,
    Positive,
    Refused,
    Adulterated,
    Invalid,
    PendingIncident
    }
    public enum TestType
    {
    PreEmployment,
    PostAccident,
    Random,
    ReasonableSuspicion,
    ReturnToDuty,
    FollowUp
    }

    public class DrugAndAlcoholTest
    {
        public int DrugAndAlcoholTestId { get; set; }
        public string DriverId { get; set; }
        public virtual Driver? Driver { get; set; } //Virtual for lazy loading
        public DateTime TestDate { get; set; }
        public TestType TestType { get; set; }
        public TestResult TestResult { get; set; }
        public string TestDetails { get; set; } // Any additional details about the test

        // Potential reference to an incident if the test is post-accident
        public int? IncidentReportId { get; set; }
        public virtual IncidentReport IncidentReport { get; set; } //Virtual for lazy loading

        // Foll-up details, if applicable
        public DateTime? FollowUpTestDate { get; set; } //Next scheduled test date
        public bool IsFollowUpComplete { get; set; } // Indicates if the follow up has been complete
    }
}
