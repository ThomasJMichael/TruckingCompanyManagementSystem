using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.enums;

namespace TCMS.Common.DTOs.Equipment
{
    public class InspectionRecordCreateDto
    {
        [Required]
        public RecordType RecordType { get; set; } = RecordType.Inspection;

        [Required]
        [StringLength(100, ErrorMessage = "The description must be at most 100 characters long.")]
        public string Description { get; set; }

        [Required]
        public DateTime MaintenanceDate { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Cost must be a positive number.")]
        public decimal Cost { get; set; }

        [Required]
        public string VehicleId { get; set; }

    }
}
