using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Equipment
{
    public class SpecialOrderPartUpdateDto
    {
        [Required(ErrorMessage = "Part ID is required.")]
        public int PartDetailsId { get; set; }

        [Required(ErrorMessage = "Part name is required.")]
        [StringLength(255, ErrorMessage = "Part name must be less than 255 characters.")]
        public string PartName { get; set; }

        [Required(ErrorMessage = "Part number is required.")]
        [StringLength(100, ErrorMessage = "Part number must be less than 100 characters.")]
        public string PartNumber { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Supplier name is required.")]
        [StringLength(255, ErrorMessage = "Supplier name must be less than 255 characters.")]
        public string Supplier { get; set; }

        // Assuming this field indicates if the part is ordered specifically for a service and not from stock
        public bool IsFromStock { get; set; } = false;

    }

}
