using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Equipment
{
    public class VehicleUpdateDto
    {
        [Required]
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
        [StringLength(255, ErrorMessage = "Brand name must be less than 255 characters.")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model is required.")]
        [StringLength(255, ErrorMessage = "Model name must be less than 255 characters.")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        [StringLength(100, ErrorMessage = "Type must be less than 100 characters.")]
        public string Type { get; set; }
    }

}
