using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCMS.Common.DTOs.Financial;
using TCMS.Common.DTOs.Shipment;
using TCMS.Common.Operations;
using TCMS.Data.Data;
using TCMS.Data.Models;
using TCMS.Services.Interfaces;

namespace TCMS.Services.Implementations
{
    public class PurchaseOrderService(TcmsContext context, IMapper mapper) : IPurchaseOrderService
    {
        public async Task<OperationResult<IEnumerable<PurchaseOrderDto>>> GetAllPurchaseOrdersAsync()
        {
            try
            {
                var purchaseOrders = await context.PurchaseOrders
                    .Include(po => po.Manifest)
                    .ThenInclude(m => m.ManifestItems)
                    .ThenInclude(mi => mi.Product)
                    .ToListAsync();

                // Step 2: Load Shipments separately
                foreach (var po in purchaseOrders)
                {
                    var shipments = await context.Shipments
                        .Where(s => s.PurchaseOrderId == po.PurchaseOrderId)
                        .Include(s => s.Manifest)
                        .ThenInclude(m => m.ManifestItems)
                        .ThenInclude(mi => mi.Product)
                        .ToListAsync();

                    po.Shipments = shipments;  // Manually associating shipments with their respective purchase orders
                }

                var purchaseOrderDtos = mapper.Map<IEnumerable<PurchaseOrderDto>>(purchaseOrders);

                return OperationResult<IEnumerable<PurchaseOrderDto>>.Success(purchaseOrderDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<PurchaseOrderDto>>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<PurchaseOrderDto>> GetPurchaseOrderByIdAsync(int id)
        {
            try
            {
                var purchaseOrder = await context.PurchaseOrders.FindAsync(id);
                if (purchaseOrder == null)
                    return OperationResult<PurchaseOrderDto>.Failure(new List<string> { "Purchase order not found." });

                var purchaseOrderDto = mapper.Map<PurchaseOrderDto>(purchaseOrder);
                return OperationResult<PurchaseOrderDto>.Success(purchaseOrderDto);
            }
            catch (Exception e)
            {
                return OperationResult<PurchaseOrderDto>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<PurchaseOrderDto>> CreatePurchaseOrderAsync(PurchaseOrderDto purchaseOrder)
        {
            try
            {
                var purchaseOrderEntity = mapper.Map<PurchaseOrder>(purchaseOrder);
                context.PurchaseOrders.Add(purchaseOrderEntity);
                await context.SaveChangesAsync();

                return OperationResult<PurchaseOrderDto>.Success(purchaseOrder);
            }
            catch (Exception e)
            {
                return OperationResult<PurchaseOrderDto>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult> UpdatePurchaseOrderAsync(PurchaseOrderDto purchaseOrder)
        {
            try
            {
                var existingPurchaseOrder = await context.PurchaseOrders.FindAsync(purchaseOrder.PurchaseOrderId);
                if (existingPurchaseOrder == null)
                    return OperationResult.Failure(new List<string> { "Purchase order not found." });

                mapper.Map(purchaseOrder, existingPurchaseOrder);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult> DeletePurchaseOrderAsync(int id)
        {
            var purchaseOrder = await context.PurchaseOrders.FindAsync(id);
            if (purchaseOrder == null)
                return OperationResult.Failure(new List<string> { "Purchase order not found." });

            try
            {
                context.PurchaseOrders.Remove(purchaseOrder);
                await context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<PurchaseOrderDto>>> GeneratePurchaseOrderReportAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }


        public async Task<OperationResult> UpdateItemStatus(UpdateItemStatusDto updatedItemStatus)
        {
            try
            {
                var purchaseOrder = await context.PurchaseOrders.FindAsync(updatedItemStatus.PurchaseOrderId);
                if (purchaseOrder == null)
                    return OperationResult.Failure(new List<string> { "Purchase order not found." });

                var manifest = await context.Manifests
                    .Include(m => m.ManifestItems)
                    .FirstOrDefaultAsync(m => m.ManifestId == purchaseOrder.ManifestId);

                if (manifest == null)
                    return OperationResult.Failure(new List<string> { "Manifest not found." });

                var item = manifest.ManifestItems.FirstOrDefault(i => i.ProductId == updatedItemStatus.ItemId);
                if (item == null)
                    return OperationResult.Failure(new List<string> { "Item not found." });

                item.Status = updatedItemStatus.Status;
                await context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message });
            }
        }


        public async Task<OperationResult<decimal>> CalculateTotalCost(int purchaseOrderId)
        {
            try
            {
                var purchaseOrder = await context.PurchaseOrders
                    .Include(po => po.Manifest) // Ensure manifests are loaded
                    .FirstOrDefaultAsync(po => po.PurchaseOrderId == purchaseOrderId);

                if (purchaseOrder == null)
                    return OperationResult<decimal>.Failure(new List<string> { "Purchase order not found." });


                var totalCost = purchaseOrder.Manifest.TotalPrice();

                if (totalCost == 0)
                    return OperationResult<decimal>.Failure(new List<string> { "Total cost is zero." });
                
                if (totalCost < 0)
                    return OperationResult<decimal>.Failure(new List<string> { "Total cost is negative." });


                return OperationResult<decimal>.Success(totalCost);
            }
            catch (Exception ex)
            {
                return OperationResult<decimal>.Failure(new List<string> { "An error occurred while calculating the total cost: " + ex.Message });
            }
        }

    }
}
