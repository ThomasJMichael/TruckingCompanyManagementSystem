using TCMS.Common.DTOs.Shipment;
using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IManifestDtoService
{
    Task<IEnumerable<ManifestDto>> GetAllManifestsAsync();
    Task<ManifestDto> GetManifestByIdAsync(int id);
    Task<ManifestDto> CreateManifestAsync(ManifestDto manifest);
    Task<bool> UpdateManifestDtoAsync(ManifestDto manifestDto);
    Task<bool> DeleteManifestDtoAsync(int id);

    Task<IEnumerable<ManifestItemDto>> GetManifestItemsByManifestIdAsync(int manifestId);
    Task<bool> AddItemToManifestAsync(int manifestId, ManifestItemDto manifestItem);
    Task<bool> RemoveItemFromManifestAsync(int manifestId, int itemId);
    Task<bool> UpdateManifestItemAsync(int manifestId, ManifestItemDto manifestItem);
    Task<bool> UpdateItemStatus(int manifestId, int itemId, ItemStatus status);
    Task<decimal> CalculateTotalCost(int manifestId);

    // Bulk operations
    Task<bool> AddItemsToManifestAsync(int manifestId, IEnumerable<ManifestItemDto> manifestItems);
    Task<bool> RemoveItemsFromManifestAsync(int manifestId, IEnumerable<int> itemIds);
    Task<bool> UpdateManifestItemsAsync(int manifestId, IEnumerable<ManifestItemDto> manifestItems);



}