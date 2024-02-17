using System.ComponentModel.DataAnnotations;
using TCMS.Common.DataAnnotations;
using TCMS.Data.Models;

namespace TCMS.Common.DTOs.DrugTestDto
{
    public class DrugTestUpdateDto
    {
        public int DrugAndAlcoholTestId { get; set; } // Assuming this is needed to identify the test to update

        [Required]
        public DateTime TestDate { get; set; }

        [Required]
        [EnumDataType(typeof(TestType))]
        public TestType TestType { get; set; }

        [Required]
        [EnumDataType(typeof(TestResult))]
        public TestResult TestResult { get; set; }

        [StringLength(500, ErrorMessage = "Test details cannot exceed 500 characters.")]
        public string? TestDetails { get; set; }

        // Optional: For linking to an incident, if the test is post-accident
        public int? IncidentReportId { get; set; }

        // For follow-up tests
        [DateInTheFuture(ErrorMessage = "Follow-up test date must be in the future.")]
        public DateTime? FollowUpTestDate { get; set; }
        public bool IsFollowUpComplete { get; set; }
    }

}
