using TCMS.Common.DTOs.Equipment;
using TCMS.Common.Operations;
using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IMaintenanceService
{
    // Maintenance Records
    Task<OperationResult<IEnumerable<MaintenanceRecordDto>>> GetAllMaintenanceRecordsAsync();
    Task<OperationResult<MaintenanceRecordDto>> GetMaintenanceRecordByIdAsync(int maintenanceRecordId);
    Task<OperationResult<IEnumerable<MaintenanceRecordDto>>> GetMaintenanceRecordsByVehicleIdAsync(int? vehicleId);
    Task<OperationResult<MaintenanceRecordDto>> CreateMaintenanceRecordAsync(MaintenanceRecordDto maintenanceRecordDto);
    Task<OperationResult> UpdateMaintenanceRecordAsync(MaintenanceRecordDto maintenanceRecordDto);
    Task<OperationResult> DeleteMaintenanceRecordAsync(int maintenanceRecordId);
    Task<OperationResult<IEnumerable<MaintenanceRecordDto>>> GetMaintenanceRecordsForPeriod(DateTime startDate, DateTime endDate);


    // Repair Records
    Task<OperationResult<IEnumerable<RepairRecordDto>>> GetAllRepairRecordsAsync();
    Task<OperationResult<RepairRecordDto>> GetRepairRecordByIdAsync(int repairRecordId);
    Task<OperationResult<RepairRecordDto>> CreateRepairRecordAsync(RepairRecordDto repairRecordDto);
    Task<OperationResult> UpdateRepairRecordAsync(RepairRecordDto repairRecordDto);
    Task<OperationResult> DeleteRepairRecordAsync(int repairRecordId);
    Task<OperationResult<IEnumerable<RepairRecordDto>>> GetRepairRecordsByVehicleIdAsync(int vehicleId);

    // Parts
    Task<OperationResult<IEnumerable<PartDetailDto>>> GetAllPartsAsync();
    Task<OperationResult<PartDetailDto>> GetPartByIdAsync(int partId);
    Task<OperationResult<PartDetailDto>> AddPartAsync(PartDetailDto partDto);
    Task<OperationResult> UpdatePartAsync(PartDetailDto partDto);
    Task<OperationResult> DeletePartAsync(int partId);

    // Assign Part to MaintenanceRecord
    Task<OperationResult> AssignPartToMaintenanceRecordAsync(int maintenanceRecordId, int partId);

    // Add Special Ordered Part
    Task<OperationResult> AddSpecialOrderedPartAsync(PartDetailDto partDto, string orderSource, decimal cost);

}


