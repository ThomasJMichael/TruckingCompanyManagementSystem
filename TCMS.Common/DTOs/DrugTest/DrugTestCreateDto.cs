using System.ComponentModel.DataAnnotations;
using TCMS.Common.DataAnnotations;
using TCMS.Common.enums;

namespace TCMS.Common.DTOs.DrugTest
{
    public class DrugTestCreateDto
    {
        public string EmployeeId { get; set; }

        public TestType TestType { get; set; }

        public TestResult TestResult { get; set; }

        public string? TestDetails { get; set; }

        public DateTime TestDate { get; set; }

        // Optional: For post-accident tests, link to an incident report
        public int? IncidentReportId { get; set; }

        public DateTime? FollowUpTestDate { get; set; }
    }
}
