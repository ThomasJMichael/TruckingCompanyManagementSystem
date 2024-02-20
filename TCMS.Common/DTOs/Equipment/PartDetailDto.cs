using System.ComponentModel.DataAnnotations;

namespace TCMS.Common.DTOs.Equipment
{
    public class PartDetailDto
    {
        public int PartDetailId { get; set; }

        [Required]
        public string PartName { get; set; }

        [Required]
        public string PartNumber { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public decimal Cost { get; set; }
    }
}
