using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using TCMS.Common.DTOs.Financial;
using TCMS.Common.DTOs.Shipment;
using TCMS.Common.Operations;
using TCMS.Services.Interfaces;

namespace TCMS.API.Controllers
{
    [Route("api/purchase-order")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public PurchaseOrderController(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        // Get all
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<PurchaseOrderDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<PurchaseOrderDto>>>> GetAllPurchaseOrders()
        {
            var result = await _purchaseOrderService.GetAllPurchaseOrdersAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get by id
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<PurchaseOrderDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<PurchaseOrderDto>>> GetPurchaseOrderById(int id)
        {
            var result = await _purchaseOrderService.GetPurchaseOrderByIdAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Create
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<PurchaseOrderDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<PurchaseOrderDto>>> CreatePurchaseOrder([FromBody] PurchaseOrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _purchaseOrderService.CreatePurchaseOrderAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Update
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<PurchaseOrderDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<PurchaseOrderDto>>> UpdatePurchaseOrder([FromBody] PurchaseOrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _purchaseOrderService.UpdatePurchaseOrderAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Delete
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> DeletePurchaseOrder(int id)
        {
            var result = await _purchaseOrderService.DeletePurchaseOrderAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Link manifest to purchase order
        [HttpPut("link-manifest")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> LinkManifestToPurchaseOrder(int manifestId,
            int purchaseOrderId)
        {
            var result = await _purchaseOrderService.LinkManifestToPurchaseOrder(manifestId, purchaseOrderId);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Update item status
        [HttpPut("update-item-status")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdateItemStatus(UpdateItemStatusDto dto)
        {
            var result = await _purchaseOrderService.UpdateItemStatus(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Calculate total cost
        [HttpGet("calculate-total-cost")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<decimal>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<decimal>>> CalculateTotalCost(int purchaseOrderId)
        {
            var result = await _purchaseOrderService.CalculateTotalCost(purchaseOrderId);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
    }


}
