using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.DTOs.Inventory;
using TCMS.Common.Operations;

namespace TCMS.Services.Interfaces
{
    public interface IInventoryService
    {
        // Inventory CRUD
        Task<OperationResult<IEnumerable<InventoryDto>>> GetAllInventoryAsync();
        Task<OperationResult<InventoryDto>> GetInventoryByProductIdAsync(int productId);
        Task<OperationResult<InventoryDto>> CreateInventoryRecordAsync(InventoryCreateDto inventoryDto);
        Task<OperationResult> UpdateInventoryAsync(InventoryUpdateDto inventoryDto);
        Task<OperationResult> DeleteInventoryAsync(int productId);

        // Inventory Adjustments
        Task<OperationResult> AdjustInventoryAsync(InventoryAdjustDto inventoryAdjustDto);
    }
}
