using System.ComponentModel.DataAnnotations;

namespace TCMS.Common.DTOs.Equipment
{
    public class MaintenanceRecordDto
    {
        public int MaintenanceRecordId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        public DateTime MaintenanceDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public decimal Cost { get; set; }

        public int[] PartIds { get; set; }
    }
}
