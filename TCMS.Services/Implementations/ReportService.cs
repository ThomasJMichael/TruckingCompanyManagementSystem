using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.DTOs.Report;
using TCMS.Common.Operations;
using TCMS.Data.Data;
using TCMS.Services.Interfaces;
using TCMS.Services.ReportSchemas;

namespace TCMS.Services.Implementations
{
    public class ReportService(
        IPayrollService payrollService,
        IMaintenanceService maintenanceService,
        IVehicleService vehicleService,
        TcmsContext context,
        IShipmentService shipmentService)
        : IReportService
    {

        public async Task<OperationResult<IEnumerable<PayrollReportDto>>> GeneratePayrollReport(ReportRequestDto requestDto)
        {
            try
            {
                var payrollResult = await payrollService.GeneratePayrollForPeriodAsync(requestDto.StartDate, requestDto.EndDate);
                if (!payrollResult.IsSuccessful)
                {
                    return OperationResult<IEnumerable<PayrollReportDto>>.Failure(payrollResult.Errors);
                }

                var payrolls = payrollResult.Data;
                var payrollReportDtos = payrolls.Select(record => new PayrollReportDto
                {
                    EmployeeId = record.EmployeeId,
                    FirstName = record.FirstName,
                    MiddleName = record.MiddleName,
                    LastName = record.LastName,
                    PayPeriodStart = record.PayPeriodStart,
                    PayPeriodEnd = record.PayPeriodEnd,
                    HoursWorked = record.HoursWorked,
                    PayRate = record.PayRate,
                    GrossPay = record.GrossPay,
                    TaxDeductions = record.TaxDeductions,
                    OtherDeductions = record.OtherDeductions,
                    NetPay = record.NetPay
                });

                return OperationResult<IEnumerable<PayrollReportDto>>.Success(payrollReportDtos.ToList());
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<PayrollReportDto>>.Failure(new List<string>
                    { "An unexpected error occurred while generating the payroll report.", ex.Message });
            }
        }


        public async Task<OperationResult<IEnumerable<MaintenanceReportDto>>> GenerateMaintenanceReport(ReportRequestDto requestDto)
        {
            try
            {
                var maintenanceResult = await maintenanceService.GetMaintenanceRecordsForPeriod(requestDto.StartDate, requestDto.EndDate);
                if (!maintenanceResult.IsSuccessful)
                {
                    return OperationResult<IEnumerable<MaintenanceReportDto>>.Failure(maintenanceResult.Errors);
                }

                var maintenanceReportDtos = maintenanceResult.Data.Select(record => new MaintenanceReportDto
                {
                    VehicleId = record.VehicleId,
                    Description = record.Description,
                    MaintenanceDate = record.MaintenanceDate,
                    Cost = record.Cost
                });

                return OperationResult<IEnumerable<MaintenanceReportDto>>.Success(maintenanceReportDtos.ToList());
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<MaintenanceReportDto>>.Failure(new List<string>
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
                    await maintenanceService.GetMaintenanceRecordsByVehicleIdAsync(requestDto.VehicleId);

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
                        record.VehicleId.ToString(),
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


        public async Task<OperationResult<IEnumerable<IncomingShipmentReportDto>>> GenerateIncomingShipmentsReport()
        {
            try
            {
                var incomingShipmentsResult = await shipmentService.GetAllIncomingShipmentsAsync();
                if (!incomingShipmentsResult.IsSuccessful)
                {
                    return OperationResult<IEnumerable<IncomingShipmentReportDto>>.Failure(incomingShipmentsResult.Errors);
                }

                var incomingShipmentsReportDtos = incomingShipmentsResult.Data.Select(shipment => new IncomingShipmentReportDto
                {
                    ShipmentId = shipment.ShipmentId,
                    ActualArrivalTime = shipment.ActualArrivalTime,
                    Company = shipment.Company,
                });


                foreach (var shipment in incomingShipmentsReportDtos)
                {
                    var shipmentModel = await context.Shipments.FindAsync(shipment.ShipmentId);
                    var purchaseOrder = await context.PurchaseOrders.FindAsync(shipmentModel.PurchaseOrderId);
                    if (purchaseOrder != null)
                    {
                        shipment.IsFullyPaid = purchaseOrder.ShippingPaid;
                    }
                }

                return OperationResult<IEnumerable<IncomingShipmentReportDto>>.Success(incomingShipmentsReportDtos.ToList());
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<IncomingShipmentReportDto>>.Failure(new List<string>
                {
                    "An unexpected error occurred while generating the incoming shipments report.",
                    ex.Message
                });
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
