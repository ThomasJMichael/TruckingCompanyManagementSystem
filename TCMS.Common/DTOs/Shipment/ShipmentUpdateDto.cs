using System;
using System.ComponentModel.DataAnnotations;

namespace TCMS.Common.DTOs.Shipment
{
    public class ShipmentUpdateDto
    {
        [Required]
        public int ShipmentId { get; set; }

        public bool hasArrived { get; set; }

        [MaxLength(100)]
        public string Company { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(2)]
        public string State { get; set; }

        [MaxLength(10)]
        public string Zip { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        public int ManifestId { get; set; }

        public DateTime? DepDateTime { get; set; }
        public DateTime? EstimatedArrivalTime { get; set; }
        public DateTime? ActualArrivalTime { get; set; }
    }
}

