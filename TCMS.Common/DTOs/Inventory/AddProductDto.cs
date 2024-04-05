using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Inventory
{
    public class AddProductDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        [Range(0.01, 1000000)]
        public decimal Price { get; set; }

        // Inventory-related information
        public int InitialQuantityOnHand { get; set; } = 0; // Default to 0 if not provided.
    }

}
