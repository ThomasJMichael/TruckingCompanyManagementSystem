﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCMS.Common.DTOs.Shipment;
using TCMS.Common.enums;
using TCMS.Common.Operations;
using TCMS.Data.Data;
using TCMS.Data.Models;
using TCMS.Services.Interfaces;

namespace TCMS.Services.Implementations
{
    public class ShipmentService(TcmsContext context, IMapper mapper, IPurchaseOrderService poService, IInventoryService inventoryService) : IShipmentService
    {
        
        public async Task<OperationResult<ShipmentCreateDto>> CreateIncomingShipment(IncomingShipmentDto incomingShipmentDto)
        {
            try
            {
                var shipment = mapper.Map<Shipment>(incomingShipmentDto);
                shipment.Direction = ShipmentDirection.Inbound;
                context.Shipments.Add(shipment);
                await context.SaveChangesAsync();

                var shipmentDto = mapper.Map<ShipmentCreateDto>(shipment);
                return OperationResult<ShipmentCreateDto>.Success(shipmentDto);

            }
            catch (Exception e)
            {
                return OperationResult<ShipmentCreateDto>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<ShipmentCreateDto>> CreateOutgoingShipment(OutgoingShipmentDto outgoingShipmentDto)
        {
            try
            {
                var shipment = mapper.Map<Shipment>(outgoingShipmentDto);
                shipment.Direction = ShipmentDirection.Outbound;
                context.Shipments.Add(shipment);
                await context.SaveChangesAsync();

                var shipmentDto = mapper.Map<ShipmentCreateDto>(shipment);
                return OperationResult<ShipmentCreateDto>.Success(shipmentDto);
            }
            catch (Exception e)
            {
                return OperationResult<ShipmentCreateDto>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult> DispatchShipment(int id)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var shipment = await context.Shipments
                    .Include(s => s.Manifest)
                    .ThenInclude(m => m.ManifestItems)
                    .SingleOrDefaultAsync(s => s.ShipmentId == id);

                if (shipment == null)
                {
                    return OperationResult.Failure(new List<string> { "Shipment not found." });
                }

                var purchaseOrder = await context.PurchaseOrders
                    .Include(po => po.Manifest)
                    .ThenInclude(m => m.ManifestItems)
                    .SingleOrDefaultAsync(po => po.PurchaseOrderId == shipment.PurchaseOrderId);

                if (purchaseOrder == null)
                {
                    return OperationResult.Failure(new List<string> { "Purchase order not found." });
                }

                foreach (var item in shipment.Manifest.ManifestItems)
                {
                    var inventoryItem = await context.Inventories.FindAsync(item.ProductId);
                    if (inventoryItem == null)
                    {
                        return OperationResult.Failure(new List<string> { "Inventory item not found." });
                    }

                    var poItem = purchaseOrder.Manifest.ManifestItems
                        .FirstOrDefault(i => i.ProductId == item.ProductId);

                    if (poItem == null)
                    {
                        return OperationResult.Failure(new List<string> { "Purchase order item not found." });
                    }

                    if (inventoryItem.QuantityOnHand < item.Quantity)
                    {
                        poItem.Status = ItemStatus.OnBackOrder;
                    }
                    else
                    {
                        poItem.Status = ItemStatus.Shipped;
                        inventoryItem.QuantityOnHand -= item.Quantity;
                    }
                }

                shipment.DepDateTime = DateTime.Now;

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return OperationResult.Failure(new List<string> { e.Message });
            }
        }


        public async Task<OperationResult> ReceiveShipment(int id)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var shipment = await context.Shipments
                    .Include(s => s.Manifest)
                    .ThenInclude(m => m.ManifestItems)
                    .SingleOrDefaultAsync(s => s.ShipmentId == id);

                if (shipment == null)
                {
                    return OperationResult.Failure(new List<string> { "Shipment not found." });
                }

                var purchaseOrder = await context.PurchaseOrders
                    .Include(po => po.Manifest)
                    .ThenInclude(m => m.ManifestItems)
                    .SingleOrDefaultAsync(po => po.PurchaseOrderId == shipment.PurchaseOrderId);

                if (purchaseOrder == null)
                {
                    return OperationResult.Failure(new List<string> { "Purchase order not found." });
                }

                foreach (var item in shipment.Manifest.ManifestItems)
                {
                    var inventoryItem = await context.Inventories.FindAsync(item.ProductId);
                    if (inventoryItem == null)
                    {
                        return OperationResult.Failure(new List<string> { "Inventory item not found." });
                    }

                    var poItem = purchaseOrder.Manifest.ManifestItems
                        .FirstOrDefault(i => i.ProductId == item.ProductId);

                    if (poItem == null)
                    {
                        return OperationResult.Failure(new List<string> { "Purchase order item not found." });
                    }

                    if (poItem.Status == ItemStatus.OnBackOrder)
                    {
                        poItem.Status = ItemStatus.Shipped;
                    }
                    inventoryItem.QuantityOnHand += item.Quantity;
                }

                shipment.ActualArrivalTime = DateTime.Now;
                shipment.hasArrived = true;

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return OperationResult.Failure(new List<string> { e.Message });
            }   
        }

        public async Task<OperationResult<IEnumerable<ShipmentDetailDto>>> GetAllShipmentsAsync()
        {
            try
            {
                var shipments = await context.Shipments
                    .Include(s => s.Manifest)
                    .Include(s => s.Vehicle)
                    .ToListAsync();

                var shipmentDtos = mapper.Map<IEnumerable<ShipmentDetailDto>>(shipments);
                return OperationResult<IEnumerable<ShipmentDetailDto>>.Success(shipmentDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<ShipmentDetailDto>>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<ShipmentDetailDto>>> GetAllIncomingShipmentsAsync()
        {
            try
            {
                var shipments = await context.Shipments
                    .Where(s => s.Direction == ShipmentDirection.Inbound)
                    .Include(s => s.PurchaseOrder)
                    .Include(s => s.Manifest)
                    .Include(s => s.Vehicle)
                    .ToListAsync();

                var shipmentDtos = mapper.Map<IEnumerable<ShipmentDetailDto>>(shipments);
                return OperationResult<IEnumerable<ShipmentDetailDto>>.Success(shipmentDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<ShipmentDetailDto>>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<ShipmentDetailDto>>> GetAllOutgoingShipmentsAsync()
        {
            try
            {
                var shipments = await context.Shipments
                    .Where(s => s.Direction == ShipmentDirection.Outbound)
                    .Include(s => s.Manifest)
                    .Include(s => s.Vehicle)
                    .ToListAsync();

                var shipmentDtos = mapper.Map<IEnumerable<ShipmentDetailDto>>(shipments);
                return OperationResult<IEnumerable<ShipmentDetailDto>>.Success(shipmentDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<ShipmentDetailDto>>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<ShipmentDetailDto>> GetShipmentByIdAsync(int id)
        {
            try
            {
                var shipment = await context.Shipments
                    .Include(s => s.Manifest)
                    .Include(s => s.Vehicle)
                    .FirstOrDefaultAsync(s => s.ShipmentId == id);

                var shipmentDto = mapper.Map<ShipmentDetailDto>(shipment);
                return OperationResult<ShipmentDetailDto>.Success(shipmentDto);
            }
            catch (Exception e)
            {
                return OperationResult<ShipmentDetailDto>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult> UpdateShipmentAsync(ShipmentUpdateDto shipmentUpdateDto)
        {
            try
            {
                var shipment = await context.Shipments.FindAsync(shipmentUpdateDto.ShipmentId);
                if (shipment == null) return OperationResult.Failure(new List<string> { "Shipment not found." });

                mapper.Map(shipmentUpdateDto, shipment);
                await context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult> DeleteShipmentAsync(int id)
        {
            try
            {
                var shipment = await context.Shipments.FindAsync(id);
                if (shipment == null) return OperationResult.Failure(new List<string> { "Shipment not found." });

                context.Shipments.Remove(shipment);
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
