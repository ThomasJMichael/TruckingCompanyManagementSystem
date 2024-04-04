using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TCMS.Common.DTOs.Inventory;
using TCMS.Common.Operations;
using TCMS.Data.Data;
using TCMS.Data.Models;
using TCMS.Services.Interfaces;

namespace TCMS.Services.Implementations
{
    public class InventoryService : IInventoryService
    {
        private readonly IMapper _mapper;
        private readonly TcmsContext _context;
        public async Task<OperationResult<IEnumerable<InventoryDto>>> GetAllInventoryAsync()
        {
            try
            {
                var inventories = _context.Inventories.ToList();
                var inventoryDtos = _mapper.Map<IEnumerable<InventoryDto>>(inventories);
                return OperationResult<IEnumerable<InventoryDto>>.Success(inventoryDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<InventoryDto>>.Failure(new List<string> {e.Message});
            }
        }

        public async Task<OperationResult<InventoryDto>> GetInventoryByProductIdAsync(int productId)
        {
            try
            {
                var inventory = _context.Inventories.FirstOrDefault(i => i.ProductId == productId);
                var inventoryDto = _mapper.Map<InventoryDto>(inventory);
                return OperationResult<InventoryDto>.Success(inventoryDto);
            }
            catch (Exception e)
            {
                return OperationResult<InventoryDto>.Failure(new List<string> { e.Message});
            }
        }

        public async Task<OperationResult<InventoryDto>> CreateInventoryRecordAsync(InventoryCreateDto inventoryDto)
        {
            try
            {
                var inventory = _mapper.Map<Inventory>(inventoryDto);
                await _context.Inventories.AddAsync(inventory);
                await _context.SaveChangesAsync();
                return OperationResult<InventoryDto>.Success(_mapper.Map<InventoryDto>(inventory));
            }
            catch (Exception e)
            {
                return OperationResult<InventoryDto>.Failure(new List<string> { e.Message});
            }
        }

        public async Task<OperationResult> UpdateInventoryAsync(InventoryUpdateDto inventoryDto)
        {
            try
            {
                var inventory = _context.Inventories.FirstOrDefault(i => i.ProductId == inventoryDto.ProductId);
                if (inventory == null)
                {
                    return OperationResult.Failure(new List<string> { "Inventory record not found" });
                }

                inventory.QuantityOnHand = inventoryDto.QuantityOnHand;
                _context.Inventories.Update(inventory);
                await _context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message});
            }
        }

        public async Task<OperationResult> DeleteInventoryAsync(int productId)
        {
            try
            {
                var inventory = _context.Inventories.FirstOrDefault(i => i.ProductId == productId);
                if (inventory == null)
                {
                    return OperationResult.Failure(new List<string> { "Inventory record not found" });
                }

                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message});
            }
        }

        public async Task<OperationResult> AdjustInventoryAsync(InventoryAdjustDto inventoryAdjustDto)
        {
            try
            {
                var inventory = _context.Inventories.FirstOrDefault(i => i.ProductId == inventoryAdjustDto.ProductId);
                if (inventory == null)
                {
                    return OperationResult.Failure(new List<string> { "Inventory record not found" });
                }

                inventory.QuantityOnHand += inventoryAdjustDto.QuantityChange;
                _context.Inventories.Update(inventory);
                await _context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message});
            }
        }
    }
}
