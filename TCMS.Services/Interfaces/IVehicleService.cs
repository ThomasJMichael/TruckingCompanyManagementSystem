﻿using TCMS.Common.DTOs.Equipment;
using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IVehicleService
{
    // Vehicle CRUD
    Task<IEnumerable<Vehicle>> GetAllAsync();
    Task<Vehicle> GetVehicleByIdAsync(int id);
    Task<Vehicle> CreateVehicleAsync(Vehicle vehicle);
    Task<bool> UpdateVehicleAsync(Vehicle vehicle);
    Task<bool> DeleteVehicleAsync(int id);

    // Routine Inspections and Maintenance
    Task<IEnumerable<MaintenanceRecordDto>> GetMaintenanceRecordsByVehicleIdAsync(string vehicleId);
    Task<IEnumerable<MaintenanceRecordDto>> GetInspectionRecordsByVehicleIdAsync(string vehicleId);
    Task<bool> AddMaintenanceRecordToVehicleAsync(string vehicleId, MaintenanceRecordDto maintenanceRecordDto);
    Task<bool> AddInspectionRecordToVehicleAsync(string vehicleId, MaintenanceRecordDto inspectionRecordDto);

    // Repair Records
    Task<IEnumerable<RepairRecord>> GetRepairRecordsByVehicleIdAsync(string vehicleId);
    Task<RepairRecord> AddRepairRecordToVehicleAsync(string vehicleId, RepairRecord repairRecord);
    Task<bool> UpdateRepairRecordAsync(RepairRecord repairRecord);
    Task<bool> DeleteRepairRecordAsync(int repairRecordId);

    // Parts Management
    Task<IEnumerable<PartDetails>> GetPartsByVehicleIdAsync(string vehicleId);
    Task<PartDetails> AddPartToVehicleAsync(string vehicleId, PartDetails part);
    Task<bool> RemovePartFromVehicleAsync(string vehicleId, int partId);

    // Special Orders
    Task<IEnumerable<PartDetails>> GetAllSpecialOrderedPartsAsync();
    Task<IEnumerable<PartDetails>> GetSpecialOrderedPartsByVehicleIdAsync(string vehicleId);
    Task<bool> MarkPartAsSpecialOrderedAsync(int partId, string orderSource, decimal cost);



}