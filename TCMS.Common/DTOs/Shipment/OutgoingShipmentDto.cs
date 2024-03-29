using System.ComponentModel.DataAnnotations;

namespace TCMS.Common.DTOs.Shipment
{
    public class OutgoingShipmentDto
    {
        [Required]
        public string Company { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip Code")]
        public string Zip { get; set; }

    }
}
