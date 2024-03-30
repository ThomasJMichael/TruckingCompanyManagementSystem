using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using TCMS.Common.DTOs.Equipment;
using TCMS.Common.Operations;
using TCMS.Data.Data;
using TCMS.Data.Models;
using TCMS.Services.Interfaces;

namespace TCMS.Services.Implementations
{
    public class VehicleService(TcmsContext context, IMapper mapper) : IVehicleService
    {
        public async Task<OperationResult<IEnumerable<VehicleDto>>> GetAllAsync()
        {
            try
            {
                var vehicles = await context.Vehicles.ToListAsync();
                var vehicleDtos = mapper.Map<IEnumerable<VehicleDto>>(vehicles);

                return OperationResult<IEnumerable<VehicleDto>>.Success(vehicleDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<VehicleDto>>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<VehicleDto>> GetVehicleByIdAsync(string vehicleId)
        {
            try
            {
                var vehicle = await context.Vehicles.FindAsync(vehicleId);
                if (vehicle == null)
                    return OperationResult<VehicleDto>.Failure(new List<string> { "Vehicle not found." });

                var vehicleDto = mapper.Map<VehicleDto>(vehicle);
                return OperationResult<VehicleDto>.Success(vehicleDto);
            }
            catch (Exception e)
            {
                return OperationResult<VehicleDto>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<VehicleDto>> CreateVehicleAsync(VehicleCreateDto vehicleDto)
        {
            try
            {
                var vehicle = mapper.Map<Vehicle>(vehicleDto);
                context.Vehicles.Add(vehicle);
                await context.SaveChangesAsync();

                var newVehicleDto = mapper.Map<VehicleDto>(vehicle);
                return OperationResult<VehicleDto>.Success(newVehicleDto);
            }
            catch (Exception e)
            {
                return OperationResult<VehicleDto>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult> UpdateVehicleAsync(VehicleUpdateDto vehicleDto)
        {
            try
            {
                var vehicle = await context.Vehicles.FindAsync(vehicleDto.VehicleId);
                if (vehicle == null)
                    return OperationResult.Failure(new List<string> { "Vehicle not found." });

                mapper.Map(vehicleDto, vehicle);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult> DeleteVehicleAsync(string vehicleId)
        {
            try
            {
                var vehicle = await context.Vehicles.FindAsync(vehicleId);
                if (vehicle == null)
                    return OperationResult.Failure(new List<string> { "Vehicle not found." });

                context.Vehicles.Remove(vehicle);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<MaintenanceRecordDto>>> GetMaintenanceRecordsByVehicleIdAsync(string vehicleId)
        {
            try
            {
                var vehicleExists = await context.Vehicles.AnyAsync(v => v.VehicleId == vehicleId);
                if (!vehicleExists)
                    return OperationResult<IEnumerable<MaintenanceRecordDto>>.Failure(new List<string> { "Vehicle not found." });

                var maintenanceRecords = await context.MaintenanceRecords
                    .Where(mr => mr.VehicleId == vehicleId)
                    .Where(mr => mr.RecordType == RecordType.Maintenance)
                    .ToListAsync();

                if (!maintenanceRecords.Any())
                    return OperationResult<IEnumerable<MaintenanceRecordDto>>.Failure(new List<string>
                        { "No maintenance records found." });

                var maintenanceRecordDtos = mapper.Map<IEnumerable<MaintenanceRecordDto>>(maintenanceRecords);

                return OperationResult<IEnumerable<MaintenanceRecordDto>>.Success(maintenanceRecordDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<MaintenanceRecordDto>>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<MaintenanceRecordDto>>> GetInspectionRecordsByVehicleIdAsync(string vehicleId)
        {
            try
            {
                var vehicleExists = await context.Vehicles.AnyAsync(v => v.VehicleId == vehicleId);
                if (!vehicleExists)
                    return OperationResult<IEnumerable<MaintenanceRecordDto>>.Failure(new List<string>
                        { "Vehicle not found." });

                var inspectionRecords = await context.MaintenanceRecords
                    .Where(mr => mr.VehicleId == vehicleId)
                    .Where(mr => mr.RecordType == RecordType.Inspection)
                    .ToListAsync();

                if (!inspectionRecords.Any())
                    return OperationResult<IEnumerable<MaintenanceRecordDto>>.Failure(new List<string>
                        { "No inspection records found." });

                var inspectionRecordDtos = mapper.Map<IEnumerable<MaintenanceRecordDto>>(inspectionRecords);

                return OperationResult<IEnumerable<MaintenanceRecordDto>>.Success(inspectionRecordDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<MaintenanceRecordDto>>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult> AddMaintenanceRecordToVehicleAsync(MaintenanceRecordCreateDto maintenanceRecordDto)
        {
            try
            {
                var vehicleExists = await context.Vehicles.AnyAsync(v => v.VehicleId == maintenanceRecordDto.VehicleId);
                if (!vehicleExists)
                    return OperationResult.Failure(new List<string> { "Vehicle not found." });

                var maintenanceRecord = mapper.Map<MaintenanceRecord>(maintenanceRecordDto);
                context.MaintenanceRecords.Add(maintenanceRecord);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult> AddInspectionRecordToVehicleAsync(InspectionRecordCreateDto inspectionRecordDto)
        {
            try
            {
                var vehicleExists = await context.Vehicles.AnyAsync(v => v.VehicleId == inspectionRecordDto.VehicleId);
                if (!vehicleExists)
                    return OperationResult.Failure(new List<string> { "Vehicle not found." });

                var inspectionRecord = mapper.Map<MaintenanceRecord>(inspectionRecordDto);
                context.MaintenanceRecords.Add(inspectionRecord);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<RepairRecordDto>>> GetRepairRecordsByVehicleIdAsync(string vehicleId)
        {
            try
            {
                var vehicleExists = await context.Vehicles.AnyAsync(v => v.VehicleId == vehicleId);
                if (!vehicleExists)
                    return OperationResult<IEnumerable<RepairRecordDto>>.Failure(new List<string>
                        { "Vehicle not found." });

                var repairRecords = await context.RepairRecords
                    .Where(rr => rr.VehicleId == vehicleId)
                    .ToListAsync();

                if (!repairRecords.Any())
                    return OperationResult<IEnumerable<RepairRecordDto>>.Failure(new List<string>
                        { "No repair records found." });

                var repairRecordDtos = mapper.Map<IEnumerable<RepairRecordDto>>(repairRecords);

                return OperationResult<IEnumerable<RepairRecordDto>>.Success(repairRecordDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<RepairRecordDto>>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<RepairRecordDto>> AddRepairRecordToVehicleAsync(RepairRecordCreateDto repairRecordDto)
        {
            try
            {
                var vehicleExists = await context.Vehicles.AnyAsync(v => v.VehicleId == repairRecordDto.VehicleId);
                if (!vehicleExists)
                    return OperationResult<RepairRecordDto>.Failure(new List<string> { "Vehicle not found." });

                var repairRecord = mapper.Map<RepairRecord>(repairRecordDto);
                context.RepairRecords.Add(repairRecord);
                await context.SaveChangesAsync();

                var newRepairRecordDto = mapper.Map<RepairRecordDto>(repairRecord);
                return OperationResult<RepairRecordDto>.Success(newRepairRecordDto);
            }
            catch (Exception e)
            {
                return OperationResult<RepairRecordDto>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult> UpdateRepairRecordAsync(RepairRecordUpdateDto repairRecordDto)
        {
            try
            {
                var repairRecord = await context.RepairRecords.FindAsync(repairRecordDto.RepairRecordId);
                if (repairRecord == null)
                    return OperationResult.Failure(new List<string> { "Repair record not found." });

                mapper.Map(repairRecordDto, repairRecord);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult> DeleteRepairRecordAsync(int repairRecordId)
        {
            try
            {
                var repairRecord = await context.RepairRecords.FindAsync(repairRecordId);
                if (repairRecord == null)
                    return OperationResult.Failure(new List<string> { "Repair record not found." });

                context.RepairRecords.Remove(repairRecord);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<PartDetailDto>>> GetPartsByVehicleIdAsync(string vehicleId)
        {
            try
            {
                var vehicleExists = await context.Vehicles.AnyAsync(v => v.VehicleId == vehicleId);
                if (!vehicleExists)
                    return OperationResult<IEnumerable<PartDetailDto>>.Failure(
                        new List<string> { "Vehicle not found." });

                var parts = await context.PartDetails
                    .Where(p => p.VehicleId == vehicleId)
                    .ToListAsync();

                if (!parts.Any())
                    return OperationResult<IEnumerable<PartDetailDto>>.Failure(new List<string> { "No parts found." });

                var partDtos = mapper.Map<IEnumerable<PartDetailDto>>(parts);

                return OperationResult<IEnumerable<PartDetailDto>>.Success(partDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<PartDetailDto>>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<PartDetailDto>> AddPartToVehicleAsync(PartDetailCreateDto partDto)
        {
            try
            {
                var vehicle = await context.Vehicles.FindAsync(partDto.VehicleId);
                if (vehicle == null)
                    return OperationResult<PartDetailDto>.Failure(new List<string> { "Vehicle not found." });

                var part = mapper.Map<PartDetails>(partDto);
                vehicle.Parts.Add(part);
                await context.SaveChangesAsync();

                var newPartDto = mapper.Map<PartDetailDto>(part);
                return OperationResult<PartDetailDto>.Success(newPartDto);
            }
            catch (Exception e)
            {
                return OperationResult<PartDetailDto>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult> RemovePartFromVehicleAsync(string vehicleId, int partId)
        {
            try
            {
                var vehicle = await context.Vehicles.FindAsync(vehicleId);
                if (vehicle == null)
                    return OperationResult.Failure(new List<string> { "Vehicle not found." });

                var part = vehicle.Parts.FirstOrDefault(p => p.PartDetailsId == partId);
                if (part == null)
                    return OperationResult.Failure(new List<string> { "Part not found." });

                vehicle.Parts.Remove(part);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<PartDetailDto>>> GetAllSpecialOrderedPartsAsync()
        {
            try
            {
                var specialOrderedParts = await context.PartDetails
                    .Where(p => !p.isFromStock)
                    .ToListAsync();

                if (!specialOrderedParts.Any())
                    return OperationResult<IEnumerable<PartDetailDto>>.Failure(new List<string>
                        { "No special ordered parts found." });

                var specialOrderedPartDtos = mapper.Map<IEnumerable<PartDetailDto>>(specialOrderedParts);

                return OperationResult<IEnumerable<PartDetailDto>>.Success(specialOrderedPartDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<PartDetailDto>>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<PartDetailDto>>> GetSpecialOrderedPartsByVehicleIdAsync(string vehicleId)
        {
            try
            {
                var vehicleExists = await context.Vehicles.AnyAsync(v => v.VehicleId == vehicleId);
                if (!vehicleExists)
                    return OperationResult<IEnumerable<PartDetailDto>>.Failure(new List<string>
                        { "Vehicle not found." });

                var specialOrderedParts = await context.PartDetails
                    .Where(p => p.VehicleId == vehicleId)
                    .Where(p => !p.isFromStock)
                    .ToListAsync();

                if (!specialOrderedParts.Any())
                    return OperationResult<IEnumerable<PartDetailDto>>.Failure(new List<string>
                        { "No special ordered parts found." });

                var specialOrderedPartDtos = mapper.Map<IEnumerable<PartDetailDto>>(specialOrderedParts);

                return OperationResult<IEnumerable<PartDetailDto>>.Success(specialOrderedPartDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<PartDetailDto>>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult> SpecialOrderedPartUpdate(SpecialOrderPartUpdateDto specialOrderDto)
        {
            try
            {
                var part = await context.PartDetails.FindAsync(specialOrderDto.PartDetailsId);
                if (part == null)
                    return OperationResult.Failure(new List<string> { "Part not found." });

                mapper.Map(specialOrderDto, part);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message });
            }
        }
    }
}
