using TCMS.Common.DTOs.Equipment;
using TCMS.Common.Operations;
using TCMS.Data.Models;
using TCMS.Common.DTOs.Equipment;

namespace TCMS.Services.Interfaces;

public interface IVehicleService
{
    // Vehicle CRUD
    Task<OperationResult<IEnumerable<VehicleDto>>> GetAllAsync();
    Task<OperationResult<VehicleDto>> GetVehicleByIdAsync(int vehicleId);
    Task<OperationResult<VehicleDto>> CreateVehicleAsync(VehicleCreateDto vehicleDto);
    Task<OperationResult> UpdateVehicleAsync(VehicleUpdateDto vehicleDto);
    Task<OperationResult> DeleteVehicleAsync(int vehicleId);

    // Routine Inspections and Maintenance
    Task<OperationResult<IEnumerable<MaintenanceRecordDto>>> GetMaintenanceRecordsByVehicleIdAsync(int vehicleId);
    Task<OperationResult<IEnumerable<MaintenanceRecordDto>>> GetInspectionRecordsByVehicleIdAsync(int vehicleId);
    Task<OperationResult> AddMaintenanceRecordToVehicleAsync(MaintenanceRecordCreateDto maintenanceRecordDto);
    Task<OperationResult> AddInspectionRecordToVehicleAsync(InspectionRecordCreateDto inspectionRecordDto);

    // Repair Records
    Task<OperationResult<IEnumerable<RepairRecordDto>>> GetRepairRecordsByVehicleIdAsync(int vehicleId);
    Task<OperationResult<RepairRecordDto>> AddRepairRecordToVehicleAsync(RepairRecordCreateDto repairRecordDto);
    Task<OperationResult> UpdateRepairRecordAsync(RepairRecordUpdateDto repairRecordDto);
    Task<OperationResult> DeleteRepairRecordAsync(int repairRecordId);

    // Parts Management
    Task<OperationResult<IEnumerable<PartDetailDto>>> GetPartsByVehicleIdAsync(int vehicleId);
    Task<OperationResult<PartDetailDto>> AddPartToVehicleAsync(PartDetailCreateDto partDto); 
    Task<OperationResult> RemovePartFromVehicleAsync(int vehicleId, int partId);

    // Special Orders
    Task<OperationResult<IEnumerable<PartDetailDto>>> GetAllSpecialOrderedPartsAsync();
    Task<OperationResult<IEnumerable<PartDetailDto>>> GetSpecialOrderedPartsByVehicleIdAsync(int vehicleId);
    Task<OperationResult> SpecialOrderedPartUpdate(SpecialOrderPartUpdateDto specialOrderDto);
}

