using System.ComponentModel.DataAnnotations;
using TCMS.Common.DataAnnotations;
using TCMS.Data.Models;

namespace TCMS.Common.DTOs.DrugTest
{
    public class DrugTestDto
    {
        [Required]
        public int DrugAndAlcoholTestId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime TestDate { get; set; }

        [Required]
        public TestType TestType { get; set; }

        [Required]
        public TestResult TestResult { get; set; }

        [StringLength(500)]
        public string TestDetails { get; set; }

        public int? IncidentReportId { get; set; }

        [StringLength(500)]
        public string IncidentDetails { get; set; } // This could include a brief summary of the incident

        // Follow-up test details, if applicable
        [DataType(DataType.Date)]
        [DateInTheFuture(ErrorMessage = "Follow-up test date must be in the future.")]
        public DateTime? FollowUpTestDate { get; set; }
        public bool IsFollowUpComplete { get; set; }
    }
}
