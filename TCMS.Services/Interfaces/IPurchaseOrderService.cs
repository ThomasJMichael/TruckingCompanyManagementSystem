using TCMS.Common.DTOs.Financial;
using TCMS.Common.DTOs.Shipment;
using TCMS.Common.Operations;
using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IPurchaseOrderService
{
    Task<OperationResult<IEnumerable<PurchaseOrderDto>>> GetAllPurchaseOrdersAsync();
    Task<OperationResult<PurchaseOrderDto>> GetPurchaseOrderByIdAsync(int id);
    Task<OperationResult<PurchaseOrderDto>> CreatePurchaseOrderAsync(PurchaseOrderDto purchaseOrder);
    Task<OperationResult> UpdatePurchaseOrderAsync(PurchaseOrderDto purchaseOrder);
    Task<OperationResult> DeletePurchaseOrderAsync(int id);

    Task<OperationResult<IEnumerable<PurchaseOrderDto>>> GeneratePurchaseOrderReportAsync(DateTime startDate, DateTime endDate);
    Task<OperationResult> UpdateItemStatus(UpdateItemStatusDto updatedItemStatus);
    Task<OperationResult<decimal>> CalculateTotalCost(int purchaseOrderId);
}
