﻿using Microsoft.AspNetCore.Mvc;
using TCMS.Common.DTOs.Inventory;
using TCMS.Common.DTOs.Shipment;
using TCMS.Common.Operations;
using TCMS.Services.Interfaces;

namespace TCMS.API.Controllers
{
    [Route("api/manifest")]
    [ApiController]
    public class ManifestController : ControllerBase
    {
        private readonly IManifestService _manifestService;

        public ManifestController(IManifestService manifestService)
        {
            _manifestService = manifestService;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<ManifestDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<ManifestDto>>>> GetAllManifests()
        {
            var result = await _manifestService.GetAllManifestsAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<ManifestDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<ManifestDto>>> GetManifestById(int id)
        {
            var result = await _manifestService.GetManifestByIdAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> CreateManifest([FromBody] ManifestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _manifestService.CreateManifestAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdateManifest([FromBody] ManifestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _manifestService.UpdateManifestAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> DeleteManifest(int id)
        {
            var result = await _manifestService.DeleteManifestAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get manifest items by manifest
        [HttpGet("{id}/items")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<ManifestItemDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<ManifestItemDto>>>> GetManifestItemsByManifest(int id)
        {
            var result = await _manifestService.GetManifestItemsByManifestIdAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Add manifest item to manifest
        [HttpPost("{id}/items/add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> AddManifestItemToManifest(int id,
            [FromBody] ManifestItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _manifestService.AddItemToManifestAsync(id, dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Remove manifest item from manifest
        [HttpDelete("{id}/items/remove")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> RemoveManifestItemFromManifest(int manifestId, int itemId)
        {
            var result = await _manifestService.RemoveItemFromManifestAsync(manifestId, itemId);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Update manifest item
        [HttpPut("{id}/items/update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdateManifestItem(int id, [FromBody] ManifestItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _manifestService.UpdateManifestItemAsync(id, dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Update Item Status
        [HttpPut("{id}/items/update/status")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdateItemStatus(UpdateItemStatusDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _manifestService.UpdateItemStatus(dto.ManifestId, dto.ItemId, dto.Status);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Calculate Manifest Total
        [HttpGet("{id}/total")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<decimal>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<decimal>>> CalculateManifestTotal(int id)
        {
            var result = await _manifestService.CalculateTotalCost(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Add product
        [HttpPost("product/add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> AddProduct(AddProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _manifestService.AddProductAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Update product
        [HttpPut("product/update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdateProduct(ProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _manifestService.UpdateProductAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Delete product
        [HttpDelete("product/delete/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> DeleteProduct(int productId)
        {
            var result = await _manifestService.DeleteProductAsync(productId);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get all products
        [HttpGet("product/all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<ProductDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<ProductDto>>>> GetAllProducts()
        {
            var result = await _manifestService.GetAllProductsAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get product by id
        [HttpGet("product/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<ProductDto>>> GetProductById(int productId)
        {
            var result = await _manifestService.GetProductByIdAsync(productId);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Add items to manifest bulk
        [HttpPost("{id}/items/add/bulk")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> AddItemsToManifestBulk(int id,
            [FromBody] IEnumerable<ManifestItemDto> dtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _manifestService.AddItemsToManifestAsync(id, dtos);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Remove items from manifest bulk
        [HttpDelete("{id}/items/remove/bulk")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> RemoveItemsFromManifestBulk(int id,
            [FromBody] IEnumerable<int> itemIds)
        {
            var result = await _manifestService.RemoveItemsFromManifestAsync(id, itemIds);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Update items in manifest bulk
        [HttpPut("{id}/items/update/bulk")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdateItemsInManifestBulk(int id,
            [FromBody] IEnumerable<ManifestItemDto> dtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _manifestService.UpdateManifestItemsAsync(id, dtos);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
    }
}
