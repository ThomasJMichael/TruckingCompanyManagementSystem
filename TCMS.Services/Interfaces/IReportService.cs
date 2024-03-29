
using TCMS.Common.DTOs.Report;
using TCMS.Common.Operations;

namespace TCMS.Services.Interfaces;

public interface IReportService
{
    // Monthly payroll including employees and total compensation
    Task<OperationResult<CsvResultDto>> GeneratePayrollReport(ReportRequestDto requestDto);
    // Total monthly maintenance performed and costs
    Task<OperationResult<CsvResultDto>> GenerateMaintenanceReport(ReportRequestDto requestDto);
    // Full maintenance history for a specific vehicle
    Task<OperationResult<CsvResultDto>> GenerateVehicleMaintenanceReport(ReportRequestDto requestDto);

    // Summary of all incoming shipments with costs, and whether payment has been submitted
    Task<OperationResult<CsvResultDto>> GenerateIncomingShipmentsReport(ReportRequestDto requestDto);

    // Summary of all outgoing shipments with costs, and whether payment has been submitted
    Task<OperationResult<CsvResultDto>> GenerateOutgoingShipmentsReport(ReportRequestDto requestDto);
}
