using Microsoft.AspNetCore.Mvc;
using TCMS.Common.DTOs.Incident;
using TCMS.Common.DTOs.Inventory;
using TCMS.Common.Operations;
using TCMS.Services.Interfaces;

namespace TCMS.API.Controllers
{
    [Route("api/inventory")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<InventoryDto>>>> GetAllInventory()
        {
            var result = await _inventoryService.GetAllInventoryAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpGet("all-details")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<InventoryProductDetailDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(OperationResult<IEnumerable<InventoryProductDetailDto>>))]
        public async Task<ActionResult<OperationResult<IEnumerable<InventoryProductDetailDto>>>> GetAllInventoryProductDetails()
        {
            var result = await _inventoryService.GetAllInventoryProductDetailsAsync();
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<OperationResult>> UpdateInventoryAndProduct([FromBody] InventoryProductDetailDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _inventoryService.UpdateInventoryAndProduct(dto);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<InventoryDto>>> GetInventoryByProductId(int productId)
        {
            var result = await _inventoryService.GetInventoryByProductIdAsync(productId);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> AddInventory([FromBody] InventoryCreateDto inventoryAddDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _inventoryService.CreateInventoryRecordAsync(inventoryAddDto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update-inventory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdateInventory(
            [FromBody] InventoryUpdateDto inventoryUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _inventoryService.UpdateInventoryAsync(inventoryUpdateDto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // delete
        [HttpDelete("delete/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> DeleteInventory(int productId)
        {
            var result = await _inventoryService.DeleteInventoryAsync(productId);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpPost("adjust")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> AdjustInventory([FromBody] InventoryAdjustDto inventoryAdjustDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _inventoryService.AdjustInventoryAsync(inventoryAdjustDto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

    }
}
