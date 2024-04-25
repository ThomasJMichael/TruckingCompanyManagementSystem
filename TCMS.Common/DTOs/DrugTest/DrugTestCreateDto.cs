using System.ComponentModel.DataAnnotations;
using TCMS.Common.DataAnnotations;
using TCMS.Common.enums;

namespace TCMS.Common.DTOs.DrugTest
{
    public class DrugTestCreateDto
    {
        [Required]
        public string EmployeeId { get; set; }

        [Required]
        [EnumDataType(typeof(TestType))]
        public TestType TestType { get; set; }

        [Required]
        [EnumDataType(typeof(TestResult))]
        public TestResult TestResult { get; set; }

        [StringLength(500, ErrorMessage = "Test details cannot exceed 500 characters.")]
        public string? TestDetails { get; set; }

        [Required]
        public DateTime TestDate { get; set; }

        // Optional: For post-accident tests, link to an incident report
        public int? IncidentReportId { get; set; }

        [DateInTheFuture(ErrorMessage = "Follow-up test date must be in the future.")]
        public DateTime? FollowUpTestDate { get; set; }
    }
}
