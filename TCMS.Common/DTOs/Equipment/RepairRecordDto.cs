using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Equipment
{
    public class RepairRecordDto
    {
        public int RepairRecordId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        public DateTime RepairDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public int[] PartIds { get; set; }
    }
}
