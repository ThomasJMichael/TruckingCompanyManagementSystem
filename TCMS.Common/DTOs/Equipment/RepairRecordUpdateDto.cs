using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Equipment
{
    public class RepairRecordUpdateDto
    {
        [Required(ErrorMessage = "The repair record ID is required for an update.")]
        public int RepairRecordId { get; set; }

        [StringLength(500, ErrorMessage = "The description must be less than 500 characters.")]
        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Cost must be a non-negative number.")]
        public decimal Cost { get; set; }

        // RepairDate may not need to be updated, consider if Required attribute is necessary.
        public DateTime? RepairDate { get; set; }

        // VehicleId may be updated if the repair record needs to be reassigned.
        public int? VehicleId { get; set; }

        [StringLength(500, ErrorMessage = "The cause must be less than 500 characters.")]
        public string Cause { get; set; }

        [StringLength(500, ErrorMessage = "The solution must be less than 500 characters.")]
        public string Solution { get; set; }
    }

}
