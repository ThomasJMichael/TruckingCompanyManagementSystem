using TCMS.Common.enums;

namespace TCMS.Data.Models
{
    
    public class MaintenanceRecord
    {
        public int MaintenanceRecordId { get; set; }
        public RecordType RecordType { get; set; }
        public string Description { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public decimal Cost { get; set; }

        //Foreign Keys
        public string? VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        public virtual ICollection<PartDetails>? PartDetails { get; set; }
    }
}