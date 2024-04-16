using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using TCMS.Common.DTOs.Inventory;
using TCMS.Common.DTOs.Shipment;
using TCMS.Common.enums;
using TCMS.Common.Operations;
using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IManifestService
{
    Task<OperationResult<IEnumerable<ManifestDto>>> GetAllManifestsAsync();
    Task<OperationResult<ManifestDto>> GetManifestByIdAsync(int id);
    Task<OperationResult<ManifestDto>> CreateManifestAsync(ManifestDto manifest);
    Task<OperationResult> UpdateManifestAsync(ManifestDto manifestDto);
    Task<OperationResult> DeleteManifestAsync(int id);

    Task<OperationResult<IEnumerable<ManifestItemDto>>> GetManifestItemsByManifestIdAsync(int manifestId);
    Task<OperationResult> AddItemToManifestAsync(int manifestId, ManifestItemDto manifestItem);
    Task<OperationResult> RemoveItemFromManifestAsync(int manifestId, int itemId);
    Task<OperationResult> UpdateManifestItemAsync(int manifestId, ManifestItemDto manifestItem);
    Task<OperationResult> UpdateItemStatus(int manifestId, int itemId, ItemStatus status);
    Task<OperationResult<decimal>> CalculateTotalCost(int manifestId);

    // Product operations
    Task<OperationResult> AddProductAsync(AddProductDto dto);
    Task<OperationResult> UpdateProductAsync(ProductDto dto);
    Task<OperationResult> DeleteProductAsync(int productId);
    Task<OperationResult<IEnumerable<ProductDto>>> GetAllProductsAsync();
    Task<OperationResult<ProductDto>> GetProductByIdAsync(int productId);


    // Bulk operations
    Task<OperationResult> AddItemsToManifestAsync(int manifestId, IEnumerable<ManifestItemDto> manifestItems);
    Task<OperationResult> RemoveItemsFromManifestAsync(int manifestId, IEnumerable<int> itemIds);
    Task<OperationResult> UpdateManifestItemsAsync(int manifestId, IEnumerable<ManifestItemDto> manifestItems);
}
