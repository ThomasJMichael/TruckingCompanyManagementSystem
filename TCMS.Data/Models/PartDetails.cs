namespace TCMS.Data.Models
{
    public class PartDetails
    {
        public int PartDetailsId { get; set; }
        public string PartName { get; set; }
        public string PartNumber { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Supplier { get; set; }
        public bool isFromStock { get; set; }

        //Foreign Keys
        public int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        public int? MaintenanceRecordId { get; set; }
        public virtual MaintenanceRecord MaintenanceRecord { get; set; }

        public int? RepairRecordId { get; set; }
        public virtual RepairRecord RepairRecord { get; set; }

    }
}