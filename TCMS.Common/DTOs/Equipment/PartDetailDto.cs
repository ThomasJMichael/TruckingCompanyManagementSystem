using System.ComponentModel.DataAnnotations;

namespace TCMS.Common.DTOs.Equipment
{
    public class PartDetailDto
    {
        public int PartDetailId { get; set; }
        public string? PartName { get; set; }
        public string? PartNumber { get; set; }
        public int? QuantityOnHand { get; set; }
        public string? Description { get; set; }
        public decimal? Cost { get; set; }
        public string? Supplier { get; set; }
        public bool? IsFromStock { get; set; }
        public int? VehicleId { get; set; }
        public int? MaintenanceRecordID { get; set; }
        public int? RepairRecordID { get; set; }
    }
}
