using TCMS.Common.DTOs.Equipment;
using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IMaintenanceService
{
    //Maintenance Records
    Task<IEnumerable<MaintenanceRecordDto>> GetAllMaintenanceRecordsAsync();
    Task<MaintenanceRecordDto> GetByIdAsync(int maintenanceRecordId);
    Task<IEnumerable<MaintenanceRecordDto>> GetByVehicleIdAsync(string vehicleId);
    Task<MaintenanceRecordDto> CreateAsync(MaintenanceRecordDto maintenanceRecordDto);
    Task<bool> UpdateAsync(MaintenanceRecordDto maintenanceRecordDto);
    Task<bool> DeleteAsync(int maintenanceRecordId);

    // Repair Records
    Task<IEnumerable<RepairRecordDto>> GetAllRepairRecordsAsync { get; }
    Task<RepairRecordDto> GetRepairRecordByIdAsync(int repairRecordId);
    Task<RepairRecordDto> CreateRepairRecordAsync(RepairRecordDto repairRecord);
    Task<bool> UpdateRepairRecordAsync(RepairRecordDto repairRecord);
    Task<bool> DeleteRepairRecordAsync(int repairRecordId);
    Task<IEnumerable<RepairRecordDto>> GetRepairRecordsByVehicleIdAsync(string vehicleId);

    // Parts
    Task<IEnumerable<PartDetailDto>> GetAllPartsAsync();
    Task<PartDetailDto> GetPartByIdAsync(int partId);
    Task<PartDetailDto> AddPartAsync(PartDetailDto part);
    Task<bool> UpdatePartAsync(PartDetailDto part);
    Task<bool> DeletePartAsync(int partId);

    // Assign Part to MaintenanceRecordDto
    Task<bool> AssignPartToMaintenanceRecordAsync(int maintenanceRecordId, int partId);

    // Add Special Ordered Part
    Task<bool> AddSpecialOrderedPartAsync(PartDetailDto part, string orderSource, decimal cost);


}