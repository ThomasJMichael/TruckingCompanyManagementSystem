using System;
using System.ComponentModel.DataAnnotations;
using TCMS.Data.Models;

namespace TCMS.Common.DTOs.Shipment
{
    public class ShipmentCreateDto
    {
        [Required]
        public ShipmentDirection Direction { get; set; }

        [Required]
        [MaxLength(100)]
        public string Company { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(2)]
        public string State { get; set; }

        [Required]
        [MaxLength(10)]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip Code")]
        public string Zip { get; set; }

        [Required]
        public string VehicleId { get; set; }

        [Required]
        public int ManifestId { get; set; }

        public DateTime? DepDateTime { get; set; }
        public DateTime? EstimatedArrivalTime { get; set; }
    }
}

