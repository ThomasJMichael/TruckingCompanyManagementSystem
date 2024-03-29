using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Equipment
{
    public class RepairRecordCreateDto
    {
        [Required(ErrorMessage = "A description of the repair is required.")]
        [StringLength(500, ErrorMessage = "The description must be less than 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Cost of repair is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Cost must be a non-negative number.")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "The date of repair is required.")]
        public DateTime RepairDate { get; set; }

        // Optional if the repair record can be created without assigning to a vehicle.
        public string? VehicleId { get; set; }

        [Required(ErrorMessage = "A cause for the repair is required.")]
        [StringLength(500, ErrorMessage = "The cause must be less than 500 characters.")]
        public string Cause { get; set; }

        [Required(ErrorMessage = "A solution for the repair is required.")]
        [StringLength(500, ErrorMessage = "The solution must be less than 500 characters.")]
        public string Solution { get; set; }
    }

}
