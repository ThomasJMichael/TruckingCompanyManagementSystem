
using TCMS.Common.DTOs.Report;

namespace TCMS.Services.Interfaces;

public interface IReportService
{
    Task<bool> GeneratePayrollReport (ReportRequestDto requestDto);
    Task<bool> GenerateMaintenanceReport (ReportRequestDto requestDto);
    Task<bool> GenerateVehicleMaintenanceReport (ReportRequestDto requestDto);

    // summary of all incoming shipments with costs, and whether payment has been submitted
    Task<bool> GenerateIncomingShipmentsReport (ReportRequestDto requestDto);
    // summary of all outgoing shipments with costs, and whether payment has been submitted
    Task<bool> GenerateOutgoingShipmentsReport (ReportRequestDto requestDto);
}