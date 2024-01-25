namespace TCMS.Data.Models
{
    public class RepairRecord
    {
        public int RepairRecordId { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public DateTime RepairDate { get; set; }
        
        //Foreign Keys
        public string? VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        public string Cause { get; set; }
        public string Solution { get; set; }
    }
}