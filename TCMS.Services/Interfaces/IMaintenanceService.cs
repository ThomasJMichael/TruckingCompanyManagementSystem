using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IMaintenanceService
{
    Task<IEnumerable<MaintenanceRecord>> GetAllAsync();
    Task<MaintenanceRecord> GetByIdAsync(int maintenanceRecordId);
    Task<IEnumerable<MaintenanceRecord>> GetByVehicleIdAsync(string vehicleId);
    Task<MaintenanceRecord> CreateAsync(MaintenanceRecord maintenanceRecord);
    Task<bool> UpdateAsync(MaintenanceRecord maintenanceRecord);
    Task<bool> DeleteAsync(int maintenanceRecordId);

    // Parts
    Task<IEnumerable<PartDetails>> GetAllPartsAsync();
    Task<PartDetails> GetPartByIdAsync(int partId);
    Task<PartDetails> AddPartAsync(PartDetails part);
    Task<bool> UpdatePartAsync(PartDetails part);
    Task<bool> DeletePartAsync(int partId);

    // Assign Part to MaintenanceRecord
    Task<bool> AssignPartToMaintenanceRecordAsync(int maintenanceRecordId, int partId);

    // Add Special Ordered Part
    Task<bool> AddSpecialOrderedPartAsync(PartDetails part, string orderSource, decimal cost);


}