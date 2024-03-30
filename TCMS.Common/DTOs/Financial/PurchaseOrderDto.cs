using System.ComponentModel.DataAnnotations;
using TCMS.Common.DTOs.Shipment;

namespace TCMS.Common.DTOs.Financial
{
    public class PurchaseOrderDto
    {
        public int PurchaseOrderId { get; set; }

        [Required] [DataType(DataType.Date)] public DateTime DateCreated { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Order number must be less than 20 characters long.")]
        public string OrderNumber { get; set; }

        // Assuming ManifestDto exists and is properly defined elsewhere
        public ICollection<ManifestDto> Manifests { get; set; } = new List<ManifestDto>();
    }
}
