﻿using AutoMapper;
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

        public MaintenanceService(TcmsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
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

        public async Task<OperationResult<IEnumerable<MaintenanceRecordDto>>> GetMaintenanceRecordsByVehicleIdAsync(int? vehicleId)
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
                var maintenanceRecord = new MaintenanceRecord()
                {
                    MaintenanceDate = maintenanceRecordDto.MaintenanceDate,
                    RecordType = maintenanceRecordDto.RecordType,
                    Cost = maintenanceRecordDto.Cost,
                    Description = maintenanceRecordDto.Description,
                    VehicleId = maintenanceRecordDto.VehicleId
                };

                _context.MaintenanceRecords.Add(maintenanceRecord);
                await _context.SaveChangesAsync();
                var newMaintenanceRecordDto = new MaintenanceRecordDto()
                {
                    MaintenanceRecordId = maintenanceRecord.MaintenanceRecordId,
                    Cost = maintenanceRecord.Cost,
                    Description = maintenanceRecord.Description,
                    MaintenanceDate = maintenanceRecord.MaintenanceDate,
                    RecordType = maintenanceRecord.RecordType,
                    VehicleId = (int)maintenanceRecord.VehicleId
                };
                return OperationResult<MaintenanceRecordDto>.Success(newMaintenanceRecordDto);
            }
            catch (Exception ex)
            {
                return OperationResult<MaintenanceRecordDto>.Failure([ ex.Message ]);
            }
        }

        public async Task<OperationResult> UpdateMaintenanceRecordAsync(MaintenanceRecordDto maintenanceRecordDto)
        {
            // Attempt to retrieve the existing record.
            var record = await _context.MaintenanceRecords
                .AsNoTracking()  
                .FirstOrDefaultAsync(r => r.MaintenanceRecordId == maintenanceRecordDto.MaintenanceRecordId);

            if (record == null)
            {
                return OperationResult.Failure(new[] { "Maintenance record not found" });
            }

            // Ensure the ID is not modified by reassigning the original ID.
            var originalId = record.MaintenanceRecordId;
            _mapper.Map(maintenanceRecordDto, record);
            record.MaintenanceRecordId = originalId; // Reset the ID in case it was changed.

            try
            {
                _context.MaintenanceRecords.Update(record); // Explicitly mark the entity as updated.
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


        public async Task<OperationResult<IEnumerable<MaintenanceRecordDto>>> GetMaintenanceRecordsForPeriod(DateTime startDate, DateTime endDate)
        {
            try
            {
                var maintenanceRecords = await _context.MaintenanceRecords
                    .Where(mr => mr.MaintenanceDate >= startDate && mr.MaintenanceDate <= endDate)
                    .ToListAsync();

                var maintenanceRecordsDto = _mapper.Map<IEnumerable<MaintenanceRecordDto>>(maintenanceRecords);
                return OperationResult<IEnumerable<MaintenanceRecordDto>>.Success(maintenanceRecordsDto);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<MaintenanceRecordDto>>.Failure([ ex.Message ]);
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

        public async Task<OperationResult<IEnumerable<RepairRecordDto>>> GetRepairRecordsByVehicleIdAsync(int vehicleId)
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

        public async Task<OperationResult<IEnumerable<PartDetailDto>>> GetPartsByMaintenanceRecordIdAsync(int maintenanceRecordId)
        {
            try
            {
                var parts = await _context.PartDetails
                    .Where(p => p.MaintenanceRecord.MaintenanceRecordId == maintenanceRecordId)
                    .ToListAsync();


                var partsDto = _mapper.Map<IEnumerable<PartDetailDto>>(parts);
                return OperationResult<IEnumerable<PartDetailDto>>.Success(partsDto);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<PartDetailDto>>.Failure([ex.Message]);
            }
        }

        public async Task<OperationResult> UpdateParts(int maintenanceRecordId, PartsDto partsDto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var vehicleId = _context.Vehicles.Where(v => v.MaintenanceRecords.Any(mr => mr.MaintenanceRecordId == maintenanceRecordId))
                    .Select(v => v.VehicleId)
                    .FirstOrDefault();

                // Handle added parts
                foreach (var partDto in partsDto.AddedParts)
                {
                    var part = new PartDetails
                    {
                        PartName = partDto.PartName,
                        PartNumber = partDto.PartNumber,
                        Price = partDto.Cost,
                        Supplier = partDto.Supplier,
                        isFromStock = (bool)partDto.IsFromStock,
                        Quantity = (int)partDto.QuantityOnHand,
                        MaintenanceRecordId = maintenanceRecordId,
                        VehicleId = vehicleId
                    };
                    _context.PartDetails.Add(part);
                }

                // Handle updated parts
                foreach (var partDto in partsDto.UpdatedParts)
                {
                    var part = await _context.PartDetails.FindAsync(partDto.PartDetailId);
                    if (part != null)
                    {
                        part.PartName = partDto.PartName;
                        part.PartNumber = partDto.PartNumber;
                        part.Price = partDto.Cost;
                        part.Supplier = partDto.Supplier;
                        part.isFromStock = (bool)partDto.IsFromStock;
                    }
                }

                // Handle removed parts
                foreach (var partDto in partsDto.RemovedParts)
                {
                    var part = await _context.PartDetails.FindAsync(partDto.PartDetailId);
                    if (part != null)
                    {
                        _context.PartDetails.Remove(part);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return OperationResult.Failure(new[] { ex.Message });
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
