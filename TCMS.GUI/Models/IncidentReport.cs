using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Data.Models;

namespace TCMS.GUI.Models
{
    public enum IncidentType
    {
        Accident,
        SafetyViolation,
        Other // Additional types can be added as needed
    }

    public class IncidentReport
    {
        public int IncidentReportId { get; set; }
        public DateTime IncidentDate { get; set; }
        public string Location { get; set; }
        public IncidentType Type { get; set; }
        public string Description { get; set; }

        // Foreign key to the involved vehicle, if applicable
        public int? VehicleId { get; set; }

        // Foreign key for the involved driver
        public string EmployeeId { get; set; }

        // Specific to accidents
        public bool IsFatal { get; set; }
        public bool HasInjuries { get; set; }
        public bool HasTowedVehicle { get; set; }
        public bool CitationIssued { get; set; }

        // Determines if a drug and alcohol test is required
        public bool RequiresDrugAndAlcoholTest => DetermineDrugAndAlcoholTestRequirement();

        // Optionally if the test record needs to be stored 
        public int? DrugAndAlcoholTestId { get; set; }

        private bool DetermineDrugAndAlcoholTestRequirement()
        {
            // Logic to determine if a test is required
            // This could be based on the type of incident and the presence of citations, injuries, or fatalities
            return Type == IncidentType.Accident && (IsFatal || (HasInjuries && CitationIssued) || (HasTowedVehicle && CitationIssued));
        }
    }
}
