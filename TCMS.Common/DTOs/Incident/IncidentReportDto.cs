using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.ComponentModel.DataAnnotations;
using TCMS.Common.Enums;

namespace TCMS.Common.DTOs.Incident
{
    public class IncidentReportDto
    {
        public int IncidentReportId { get; set; }

        public DateTime IncidentDate { get; set; }

        public string Location { get; set; }

        public IncidentType Type { get; set; }

        public string Description { get; set; }

        public int? VehicleId { get; set; }

        public string EmployeeId { get; set; }

        public bool IsFatal { get; set; }

        public bool HasInjuries { get; set; }

        public bool HasTowedVehicle { get; set; }

        public bool CitationIssued { get; set; }

        public bool RequiresDrugAndAlcoholTest { get; set; }

        public int? DrugAndAlcoholTestId { get; set; }
    }
}

