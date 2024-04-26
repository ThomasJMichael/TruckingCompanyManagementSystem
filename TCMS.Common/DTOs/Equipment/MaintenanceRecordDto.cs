using System.ComponentModel.DataAnnotations;

namespace TCMS.Common.DTOs.Equipment
{
    public class MaintenanceRecordDto
    {
        public int MaintenanceRecordId { get; set; }

        public int VehicleId { get; set; }

        public DateTime MaintenanceDate { get; set; }

        public string? Description { get; set; }

        public decimal Cost { get; set; }

        public int[]? PartIds { get; set; }
    }
}
