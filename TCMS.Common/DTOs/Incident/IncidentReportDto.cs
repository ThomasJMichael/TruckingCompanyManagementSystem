using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.ComponentModel.DataAnnotations;
using TCMS.Data.Models;

namespace TCMS.Common.DTOs.Incident
{
    public class IncidentReportDto
    {
        public int IncidentReportId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Incident Date")]
        public DateTime IncidentDate { get; set; }

        [StringLength(100)]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [EnumDataType(typeof(IncidentType))]
        [Display(Name = "Incident Type")]
        public IncidentType Type { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [StringLength(50)]
        [Display(Name = "Vehicle ID")]
        public string? VehicleId { get; set; }

        [Required]
        [Display(Name = "Driver ID")]
        public string EmployeeId { get; set; }

        [Display(Name = "Is Fatal")]
        public bool IsFatal { get; set; }

        [Display(Name = "Has Injuries")]
        public bool HasInjuries { get; set; }

        [Display(Name = "Has Towed Vehicle")]
        public bool HasTowedVehicle { get; set; }

        [Display(Name = "Citation Issued")]
        public bool CitationIssued { get; set; }

        [Display(Name = "Requires Drug and Alcohol Test")]
        public bool RequiresDrugAndAlcoholTest { get; set; }

        [Display(Name = "Drug and Alcohol Test ID")]
        public int? DrugAndAlcoholTestId { get; set; }
    }
}

