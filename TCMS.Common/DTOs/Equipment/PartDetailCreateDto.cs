using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Equipment
{
    public class PartDetailCreateDto
    {
        [Required(ErrorMessage = "Part name is required.")]
        [StringLength(100, ErrorMessage = "Part name must be less than 100 characters.")]
        public string PartName { get; set; }

        [Required(ErrorMessage = "Part number is required.")]
        [StringLength(50, ErrorMessage = "Part number must be less than 50 characters.")]
        public string PartNumber { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [StringLength(100, ErrorMessage = "Supplier name must be less than 100 characters.")]
        public string Supplier { get; set; }

        [Required]
        public bool IsFromStock { get; set; }

        // Optional if you allow creating a part without linking to a vehicle initially.
        public int? VehicleId { get; set; }

        // These can be null if the part is not being created in the context of a maintenance or repair record.
        public int? MaintenanceRecordId { get; set; }
        public int? RepairRecordId { get; set; }
    }

}
