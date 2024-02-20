using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCMS.Common.DTOs.Equipment;
using TCMS.Common.Operations;
using TCMS.Data.Data;
using TCMS.Data.Models;
using TCMS.Services.Interfaces;

namespace TCMS.Services.Implementations
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly TcmsContext _context;
        private readonly IMapper _mapper;
        public async Task<OperationResult<IEnumerable<MaintenanceRecordDto>>> GetAllMaintenanceRecordsAsync()
        {
            try
            {
                var maintenanceRecords = await _context.MaintenanceRecords.ToListAsync();
                var maintenanceRecordsDto = _mapper.Map<IEnumerable<MaintenanceRecordDto>>(maintenanceRecords);
                return OperationResult<IEnumerable<MaintenanceRecordDto>>.Success(maintenanceRecordsDto);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<MaintenanceRecordDto>>.Failure([ ex.Message ]);
            }
        }

        public async Task<OperationResult<MaintenanceRecordDto>> GetMaintenanceRecordByIdAsync(int maintenanceRecordId)
        {
            try
            {
                var maintenanceRecord = await _context.MaintenanceRecords.FindAsync(maintenanceRecordId);
                if (maintenanceRecord == null)
                    return OperationResult<MaintenanceRecordDto>.Failure(["Maintenance record not found"]);

                var maintenanceRecordDto = _mapper.Map<MaintenanceRecordDto>(maintenanceRecord);
                return OperationResult<MaintenanceRecordDto>.Success(maintenanceRecordDto);
            }
            catch (Exception ex)
            {
                return OperationResult<MaintenanceRecordDto>.Failure([ ex.Message ]);
            }
        }

        public async Task<OperationResult<IEnumerable<MaintenanceRecordDto>>> GetMaintenanceRecordsByVehicleIdAsync(string vehicleId)
        {
            try
            {
                var maintenanceRecords = 
                    await _context.MaintenanceRecords.Where(mr => mr.VehicleId == vehicleId).ToListAsync();
                var maintenanceRecordsDto = _mapper.Map<IEnumerable<MaintenanceRecordDto>>(maintenanceRecords);
                return OperationResult<IEnumerable<MaintenanceRecordDto>>.Success(maintenanceRecordsDto);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<MaintenanceRecordDto>>.Failure([ ex.Message ]);
            }
        }

        public async Task<OperationResult<MaintenanceRecordDto>> CreateMaintenanceRecordAsync(MaintenanceRecordDto maintenanceRecordDto)
        {
            try
            {
                var maintenanceRecord = _mapper.Map<MaintenanceRecord>(maintenanceRecordDto);
                _context.MaintenanceRecords.Add(maintenanceRecord);
                await _context.SaveChangesAsync();
                var newMaintenanceRecordDto = _mapper.Map<MaintenanceRecordDto>(maintenanceRecord);
                return OperationResult<MaintenanceRecordDto>.Success(newMaintenanceRecordDto);
            }
            catch (Exception ex)
            {
                return OperationResult<MaintenanceRecordDto>.Failure([ ex.Message ]);
            }
        }

        public async Task<OperationResult> UpdateMaintenanceRecordAsync(MaintenanceRecordDto maintenanceRecordDto)
        {
            var record = await _context.MaintenanceRecords.FindAsync(maintenanceRecordDto.MaintenanceRecordId);
            if(record == null) return OperationResult.Failure(new[] { "Maintenance record not found" });

            _mapper.Map(maintenanceRecordDto, record);

            try
            {
                await _context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure(new[] { ex.Message });
            }
        }

        public async Task<OperationResult> DeleteMaintenanceRecordAsync(int maintenanceRecordId)
        {
            try
            {
                var maintenanceRecord = await _context.MaintenanceRecords.FindAsync(maintenanceRecordId);
                if (maintenanceRecord == null) return OperationResult.Failure(new[] { "Maintenance record not found" });

                _context.MaintenanceRecords.Remove(maintenanceRecord);
                await _context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure(new[] { ex.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<RepairRecordDto>>> GetAllRepairRecordsAsync()
        {
            try
            {
                var repairRecords = await _context.RepairRecords.ToListAsync();
                var repairRecordsDto = _mapper.Map<IEnumerable<RepairRecordDto>>(repairRecords);
                return OperationResult<IEnumerable<RepairRecordDto>>.Success(repairRecordsDto);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<RepairRecordDto>>.Failure([ ex.Message ]);
            }
        }

        public async Task<OperationResult<RepairRecordDto>> GetRepairRecordByIdAsync(int repairRecordId)
        {
            try
            {
                var repairRecord = await _context.RepairRecords.FindAsync(repairRecordId);
                if (repairRecord == null) return OperationResult<RepairRecordDto>.Failure(["Repair record not found"]);

                var repairRecordDto = _mapper.Map<RepairRecordDto>(repairRecord);
                return OperationResult<RepairRecordDto>.Success(repairRecordDto);
            }
            catch (Exception ex)
            {
                return OperationResult<RepairRecordDto>.Failure([ex.Message]);
            }
        }

        public async Task<OperationResult<RepairRecordDto>> CreateRepairRecordAsync(RepairRecordDto repairRecordDto)
        {
            try
            {
                var repairRecord = _mapper.Map<RepairRecord>(repairRecordDto);
                _context.RepairRecords.Add(repairRecord);
                await _context.SaveChangesAsync();
                var newRepairRecordDto = _mapper.Map<RepairRecordDto>(repairRecord);
                return OperationResult<RepairRecordDto>.Success(newRepairRecordDto);
            }
            catch (Exception ex)
            {
                return OperationResult<RepairRecordDto>.Failure([ex.Message]);
            }
        }

        public async Task<OperationResult> UpdateRepairRecordAsync(RepairRecordDto repairRecordDto)
        {
            try
            {
                var record = await _context.RepairRecords.FindAsync(repairRecordDto.RepairRecordId);
                if (record == null) return OperationResult.Failure(new[] { "Repair record not found" });

                _mapper.Map(repairRecordDto, record);
                await _context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure(new[] { ex.Message });
            }
        }

        public async Task<OperationResult> DeleteRepairRecordAsync(int repairRecordId)
        {
            try
            {
                var repairRecord = await _context.RepairRecords.FindAsync(repairRecordId);
                if (repairRecord == null) return OperationResult.Failure(new[] { "Repair record not found" });

                _context.RepairRecords.Remove(repairRecord);
                await _context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure(new[] { ex.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<RepairRecordDto>>> GetRepairRecordsByVehicleIdAsync(string vehicleId)
        {
            try
            {
                var repairRecords = await _context.RepairRecords.Where(rr => rr.VehicleId == vehicleId).ToListAsync();
                var repairRecordsDto = _mapper.Map<IEnumerable<RepairRecordDto>>(repairRecords);
                return OperationResult<IEnumerable<RepairRecordDto>>.Success(repairRecordsDto);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<RepairRecordDto>>.Failure([ ex.Message ]);
            }
        }

        public async Task<OperationResult<IEnumerable<PartDetailDto>>> GetAllPartsAsync()
        {
            try
            {
                var parts = await _context.PartDetails.ToListAsync();
                var partsDto = _mapper.Map<IEnumerable<PartDetailDto>>(parts);
                return OperationResult<IEnumerable<PartDetailDto>>.Success(partsDto);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<PartDetailDto>>.Failure([ ex.Message ]);
            }
        }

        public async Task<OperationResult<PartDetailDto>> GetPartByIdAsync(int partId)
        {
            try
            {
                var part = await _context.PartDetails.FindAsync(partId);
                if (part == null) return OperationResult<PartDetailDto>.Failure(["Part not found"]);

                var partDto = _mapper.Map<PartDetailDto>(part);
                return OperationResult<PartDetailDto>.Success(partDto);
            }
            catch (Exception ex)
            {
                return OperationResult<PartDetailDto>.Failure([ ex.Message ]);
            }
        }

        public async Task<OperationResult<PartDetailDto>> AddPartAsync(PartDetailDto partDto)
        {
            try
            {
                var part = _mapper.Map<PartDetails>(partDto);
                _context.PartDetails.Add(part);
                await _context.SaveChangesAsync();
                var newPartDto = _mapper.Map<PartDetailDto>(part);
                return OperationResult<PartDetailDto>.Success(newPartDto);
            }
            catch (Exception ex)
            {
                return OperationResult<PartDetailDto>.Failure([ ex.Message ]);
            }
        }

        public async Task<OperationResult> UpdatePartAsync(PartDetailDto partDto)
        {
            try
            {
                var part = await _context.PartDetails.FindAsync(partDto.PartDetailId);
                if (part == null) return OperationResult.Failure(new[] { "Part not found" });

                _mapper.Map(partDto, part);
                await _context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure(new[] { ex.Message });
            }
        }

        public async Task<OperationResult> DeletePartAsync(int partId)
        {
            try
            {
                var part = await _context.PartDetails.FindAsync(partId);
                if (part == null) return OperationResult.Failure(new[] { "Part not found" });

                _context.PartDetails.Remove(part);
                await _context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure(new[] { ex.Message });
            }
        }

        public async Task<OperationResult> AssignPartToMaintenanceRecordAsync(int maintenanceRecordId, int partId)
        {
            await using var transaction = _context.Database.BeginTransaction(); 

            try
            {
                var maintenanceRecord = await _context.MaintenanceRecords.FindAsync(maintenanceRecordId);
                if (maintenanceRecord == null) return OperationResult.Failure(new[] { "Maintenance record not found" });

                var part = await _context.PartDetails.FindAsync(partId);
                if (part == null) return OperationResult.Failure(new[] { "Part not found" });

                maintenanceRecord.PartDetails.Add(part);
                await _context.SaveChangesAsync();
                transaction.Commit();
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return OperationResult.Failure(new[] { ex.Message });
            }
        }

        public async Task<OperationResult> AddSpecialOrderedPartAsync(PartDetailDto partDto, string orderSource, decimal cost)
        {
            try
            {
                var part = _mapper.Map<PartDetails>(partDto);
                part.Price = cost;
                part.Supplier = orderSource;
                part.isFromStock = false;
                _context.PartDetails.Add(part);
                await _context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure(new[] { ex.Message });
            }
        }
    }
}
