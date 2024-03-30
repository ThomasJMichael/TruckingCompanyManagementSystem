using System.ComponentModel.DataAnnotations;

namespace TCMS.Common.DTOs.Report
{
    public class ReportRequestDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? VehicleId { get; set; } 
    }
}
