using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.DTOs.Incident;
using TCMS.Common.Operations;

namespace TCMS.Services.Interfaces;

public interface IIncidentService
{
    Task<OperationResult<IncidentReportDto>> CreateIncidentReportAsync(IncidentReportDto incidentReportDto);
    Task<OperationResult> UpdateIncidentReportAsync(IncidentReportDto incidentReportDto);
    Task<OperationResult> DeleteIncidentReportAsync(int incidentReportId);
    Task<OperationResult<IEnumerable<IncidentReportDto>>> GetAllIncidentReportsAsync();
    Task<OperationResult<IncidentReportDto>> GetIncidentReportByIdAsync(int incidentReportId);

    // This method will be responsible for checking if a Drug and Alcohol Test is needed after an incident
    Task<OperationResult> EvaluateAndInitiateDrugTestForIncidentAsync(int incidentReportId, DateTime? testDate = null );
}

