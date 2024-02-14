using TCMS.Common.DTOs.Financial;
using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IPurchaseOrderDtoService
{
    Task<IEnumerable<PurchaseOrderDto>> GetAllPurchaseOrdersAsync();
    Task<PurchaseOrderDto> GetPurchaseOrderByIdAsync(int id);
    Task<PurchaseOrderDto> CreatePurchaseOrderAsync(PurchaseOrderDto purchaseOrder);
    Task<bool> UpdatePurchaseOrderDtoAsync(PurchaseOrderDto purchaseOrder);
    Task<bool> DeletePurchaseOrderDtoAsync(int id);

    Task<IEnumerable<PurchaseOrderDto>> GeneratePurchaseOrderReportAsync(DateTime startDate, DateTime endDate);
    Task<bool> LinkManifestToPurchaseOrderDto(int manifestId, int purchaseOrderId);
    Task<bool> UpdateItemStatus(int purchaseOrderId, int itemId, ItemStatus status);
    Task<decimal> CalculateTotalCost(int purchaseOrderId);

}