using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IPurchaseOrderService
{
    Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync();
    Task<PurchaseOrder> GetPurchaseOrderByIdAsync(int id);
    Task<PurchaseOrder> CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
    Task<bool> UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
    Task<bool> DeletePurchaseOrderAsync(int id);

    Task<IEnumerable<PurchaseOrder>> GeneratePurchaseOrderReportAsync(DateTime startDate, DateTime endDate);
    Task<bool> LinkManifestToPurchaseOrder(int manifestId, int purchaseOrderId);
    Task<bool> UpdateItemStatus(int purchaseOrderId, ManifestItem itemId, ItemStatus status);
    Task<decimal> CalculateTotalCost(int purchaseOrderId);

}