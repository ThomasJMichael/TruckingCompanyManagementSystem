using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.DTOs.Report;
using TCMS.Common.Operations;
using TCMS.Services.Interfaces;
using TCMS.Services.ReportSchemas;

namespace TCMS.Services.Implementations
{
    public class ReportService(
        IPayrollService payrollService,
        IMaintenanceService maintenanceService,
        IVehicleService vehicleService,
        IShipmentService shipmentService)
        : IReportService
    {

        public async Task<OperationResult<CsvResultDto>> GeneratePayrollReport(ReportRequestDto requestDto)
        {
            try
            {
                var payroll =
                    await payrollService.GeneratePayrollForPeriodAsync(requestDto.StartDate, requestDto.EndDate);
                if (!payroll.IsSuccessful)
                {
                    return OperationResult<CsvResultDto>.Failure(payroll.Errors);
                }

                var payrolls = payroll.Data.ToList();

                var csv = new StringBuilder();
                csv.AppendLine(string.Join(",", PayrollReportSchema.Schema.Fields));

                foreach (var record in payrolls)
                {
                    csv.AppendLine(string.Join(",", new string[]
                    {
                        record.EmployeeId.ToString(),
                        record.FirstName,
                        record.MiddleName ?? string.Empty,
                        record.LastName,
                        record.PayPeriodStart.ToString("yyyy-MM-dd"),
                        record.PayPeriodEnd.ToString("yyyy-MM-dd"),
                        record.HoursWorked.ToString(),
                        record.PayRate.ToString("F2"),
                        record.GrossPay.ToString("F2"),
                        record.TaxDeductions.ToString("F2"),
                        record.OtherDeductions.ToString("F2"),
                        record.NetPay.ToString("F2"),
                    }));
                }

                var resultDto = new CsvResultDto(csv.ToString(), PayrollReportSchema.Schema.Filename);
                return OperationResult<CsvResultDto>.Success(resultDto);
            }
            catch (Exception)
            {
                return OperationResult<CsvResultDto>.Failure(new List<string>
                    { "An unexpected error occurred while generating the payroll report." });
            }

        }

        public async Task<OperationResult<CsvResultDto>> GenerateMaintenanceReport(ReportRequestDto requestDto)
        {
            try
            {
                var maintenanceRecords =
                    await maintenanceService.GetMaintenanceRecordsForPeriod(requestDto.StartDate, requestDto.EndDate);
                if (!maintenanceRecords.IsSuccessful)
                {
                    return OperationResult<CsvResultDto>.Failure(maintenanceRecords.Errors);
                }

                var records = maintenanceRecords.Data.ToList();

                var csv = new StringBuilder();
                csv.AppendLine(string.Join(",", MaintenanceReportSchema.Schema.Fields));

                foreach (var record in records)
                {
                    csv.AppendLine(string.Join(",", new string[]
                    {
                        record.VehicleId,
                        record.Description,
                        record.MaintenanceDate.ToString("yyyy-MM-dd"),
                        record.Cost.ToString("F2"),
                    }));
                }

                var resultDto = new CsvResultDto(csv.ToString(), MaintenanceReportSchema.Schema.Filename);
                return OperationResult<CsvResultDto>.Success(resultDto);
            }
            catch (Exception ex)
            {
                return OperationResult<CsvResultDto>.Failure(new List<string>
                    { "An unexpected error occurred while generating the maintenance report.", ex.Message });
            }
        }


        public async Task<OperationResult<CsvResultDto>> GenerateVehicleMaintenanceReport(ReportRequestDto requestDto)
        {
            try
            {
                if (requestDto.VehicleId is null)
                {
                    return OperationResult<CsvResultDto>.Failure(new List<string> { "Invalid vehicle ID." });
                }

                // Assuming requestDto includes VehicleId, StartDate, and EndDate
                var maintenanceRecords =
                    await maintenanceService.GetMaintenanceRecordsByVehicleIdAsync(vehicleId: requestDto.VehicleId);

                if (!maintenanceRecords.IsSuccessful)
                {
                    return OperationResult<CsvResultDto>.Failure(new List<string>
                        { "No maintenance records found for the specified period and vehicle." });
                }

                var csv = new StringBuilder();
                csv.AppendLine(string.Join(",", VehicleMaintenanceReportSchema.Schema.Fields));

                foreach (var record in maintenanceRecords.Data)
                {
                    csv.AppendLine(string.Join(",", new string[]
                    {
                        record.VehicleId,
                        record.MaintenanceDate.ToString("yyyy-MM-dd"),
                        record.Description,
                        record.Cost.ToString("F2"),
                    }));
                }

                return OperationResult<CsvResultDto>.Success(new CsvResultDto(csv.ToString(), VehicleMaintenanceReportSchema.Schema.Filename));
            }

            catch (Exception ex)
            {
                return OperationResult<CsvResultDto>.Failure(new List<string> { ex.Message });
            }
        }


    public async Task<OperationResult<CsvResultDto>> GenerateIncomingShipmentsReport(ReportRequestDto requestDto)
        {
            try
            {
                var incomingShipments =
                    await shipmentService.GetAllIncomingShipmentsAsync();
                if (!incomingShipments.IsSuccessful)
                {
                    return OperationResult<CsvResultDto>.Failure(incomingShipments.Errors);
                }

                var shipments = incomingShipments.Data.ToList();

                var csv = new StringBuilder();
                csv.AppendLine(string.Join(",", IncomingShipmentReportSchema.Schema.Fields));

                foreach (var shipment in shipments)
                {
                    csv.AppendLine(string.Join(",", new string[]
                    {
                        shipment.ShipmentId.ToString(),
                        shipment.ActualArrivalTime.ToString(),
                        shipment.Company,
                        shipment.TotalCost.ToString("F2"),
                        shipment.IsFullyPaid ? "Yes" : "No",
                    }));
                }

                var resultDto = new CsvResultDto(csv.ToString(), IncomingShipmentReportSchema.Schema.Filename);
                return OperationResult<CsvResultDto>.Success(resultDto);
            }
            catch (Exception ex)
            {
                return OperationResult<CsvResultDto>.Failure(new List<string>
                { "An unexpected error occurred while generating the incoming shipments report.", ex.Message });
            }
        }

        public async Task<OperationResult<CsvResultDto>> GenerateOutgoingShipmentsReport(ReportRequestDto requestDto)
        {
            try
            {
                var outgoingShipments =
                    await shipmentService.GetAllOutgoingShipmentsAsync();
                if (!outgoingShipments.IsSuccessful)
                {
                    return OperationResult<CsvResultDto>.Failure(outgoingShipments.Errors);
                }

                var shipments = outgoingShipments.Data.ToList();

                var csv = new StringBuilder();
                csv.AppendLine(string.Join(",", OutgoingShipmentReportSchema.Schema.Fields));

                foreach (var shipment in shipments)
                {
                    csv.AppendLine(string.Join(",", new string[]
                    {
                        shipment.ShipmentId.ToString(),
                        shipment.DepDateTime.ToString(),
                        shipment.Address + " " + shipment.City + " " + shipment.State + " " + shipment.Zip,
                        shipment.TotalCost.ToString("F2"),
                        shipment.IsFullyPaid ? "Yes" : "No",
                    }));
                }

                var resultDto = new CsvResultDto(csv.ToString(), OutgoingShipmentReportSchema.Schema.Filename);
                return OperationResult<CsvResultDto>.Success(resultDto);
            }
            catch (Exception ex)
            {
                return OperationResult<CsvResultDto>.Failure(new List<string>
                { "An unexpected error occurred while generating the outgoing shipments report.", ex.Message });
            }
        }
    }
}
