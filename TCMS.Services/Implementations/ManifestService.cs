using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCMS.Common.DTOs.Shipment;
using TCMS.Common.Operations;
using TCMS.Data.Data;
using TCMS.Data.Models;
using TCMS.Services.Interfaces;

namespace TCMS.Services.Implementations
{
    public class ManifestService : IManifestService
    {
        private readonly TcmsContext context;
        private readonly IMapper mapper;

        public ManifestService(TcmsContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<OperationResult<IEnumerable<ManifestDto>>> GetAllManifestsAsync()
        {
            try
            {
                var manifests = await context.Manifests.ToListAsync();

                var manifestDtos = mapper.Map<IEnumerable<ManifestDto>>(manifests);

                return OperationResult<IEnumerable<ManifestDto>>.Success(manifestDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<ManifestDto>>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<ManifestDto>> GetManifestByIdAsync(int id)
        {
            try
            {
                var manifest = await context.Manifests.FindAsync(id);
                if (manifest == null) return OperationResult<ManifestDto>.Failure(["Manifest not found."]);

                var manifestDto = mapper.Map<ManifestDto>(manifest);
                return OperationResult<ManifestDto>.Success(manifestDto);
            }
            catch (Exception e)
            {
                return OperationResult<ManifestDto>.Failure([e.Message]);
            }
        }

        public async Task<OperationResult<ManifestDto>> CreateManifestAsync(ManifestDto manifest)
        {
            try
            {
                var newManifest = mapper.Map<Manifest>(manifest);
                context.Manifests.Add(newManifest);
                await context.SaveChangesAsync();

                var newManifestDto = mapper.Map<ManifestDto>(newManifest);
                return OperationResult<ManifestDto>.Success(newManifestDto);
            }
            catch (Exception e)
            {
                return OperationResult<ManifestDto>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult> UpdateManifestAsync(ManifestDto manifestDto)
        {
            try
            {
                var manifest = await context.Manifests.FindAsync(manifestDto.ManifestId);
                if (manifest == null) return OperationResult.Failure(new[] { "Manifest not found" });

                mapper.Map(manifestDto, manifest);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new[] { e.Message });
            }
        }

        public async Task<OperationResult> DeleteManifestAsync(int id)
        {
            try
            {
                var manifest = await context.Manifests.FindAsync(id);
                if (manifest == null) return OperationResult.Failure(new[] { "Manifest not found" });

                context.Manifests.Remove(manifest);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new[] { e.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<ManifestItemDto>>> GetManifestItemsByManifestIdAsync(int manifestId)
        {
            try
            {
                var manifestItems = await context.ManifestItems
                    .Where(item => item.ManifestId == manifestId)
                    .ToListAsync();

                var manifestItemDtos = mapper.Map<IEnumerable<ManifestItemDto>>(manifestItems);
                return OperationResult<IEnumerable<ManifestItemDto>>.Success(manifestItemDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<ManifestItemDto>>.Failure([e.Message]);
            }
        }

        public async Task<OperationResult> AddItemToManifestAsync(int manifestId, ManifestItemDto manifestItem)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var manifest = await context.Manifests.FindAsync(manifestId);
                if (manifest == null) return OperationResult.Failure(new[] { "Manifest not found" });

                var product = await context.Products.FindAsync(manifestItem.ProductId);
                if (product == null) return OperationResult.Failure(new[] { "Product not found" });

                var newManifestItem = mapper.Map<ManifestItem>(manifestItem);
                newManifestItem.ManifestId = manifestId;

                context.ManifestItems.Add(newManifestItem);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return OperationResult.Failure(new[] { e.Message });
            }
        }

        public async Task<OperationResult> RemoveItemFromManifestAsync(int manifestId, int itemId)
        {
            try
            {
                var manifestItem = await context.ManifestItems.FindAsync(itemId);
                if (manifestItem == null) return OperationResult.Failure(new[] { "Manifest item not found" });

                context.ManifestItems.Remove(manifestItem);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new[] { e.Message });
            }
        }

        public async Task<OperationResult> UpdateManifestItemAsync(int manifestId, ManifestItemDto manifestItem)
        {
            try
            {
                var manifest = await context.Manifests.FindAsync(manifestId);
                if (manifest == null) return OperationResult.Failure(new[] { "Manifest not found" });

                var item = await context.ManifestItems.FindAsync(manifestItem.ManifestItemId);
                if (item == null) return OperationResult.Failure(new[] { "Manifest item not found" });

                var product = await context.Products.FindAsync(manifestItem.ProductId);
                if (product == null) return OperationResult.Failure(new[] { "Product not found" });

                mapper.Map(manifestItem, item);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new[] { e.Message });
            }
        }

        public async Task<OperationResult> UpdateItemStatus(int manifestId, int itemId, ItemStatus status)
        {
            try
            {
                var manifest = await context.Manifests.FindAsync(manifestId);
                if (manifest == null) return OperationResult.Failure(new[] { "Manifest not found" });

                var item = await context.ManifestItems.FindAsync(itemId);
                if (item == null) return OperationResult.Failure(new[] { "Manifest item not found" });

                item.Status = status;
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new[] { e.Message });
            }
        }

        public async Task<OperationResult<decimal>> CalculateTotalCost(int manifestId)
        {
            try
            {
                var manifestItems = await context.ManifestItems
                    .Where(item => item.ManifestId == manifestId)
                    .ToListAsync();

                var totalCost = manifestItems.Sum(item => item.Quantity * item.Price);
                return OperationResult<decimal>.Success(totalCost);
            }
            catch (Exception e)
            {
                return OperationResult<decimal>.Failure([e.Message]);
            }
        }

        public async Task<OperationResult> AddProductAsync(ProductDto dto)
        {
            try
            {
                var product = mapper.Map<Product>(dto);
                context.Products.Add(product);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new[] { e.Message });
            }
        }

        public async Task<OperationResult> UpdateProductAsync(ProductDto dto)
        {
            try
            {
                var product = await context.Products.FindAsync(dto.ProductId);
                if (product == null) return OperationResult.Failure(new[] { "Product not found" });

                mapper.Map(dto, product);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new[] { e.Message });
            }
        }

        public async Task<OperationResult> DeleteProductAsync(int productId)
        {
            try
            {
                var product = await context.Products.FindAsync(productId);
                if (product == null) return OperationResult.Failure(new[] { "Product not found" });

                context.Products.Remove(product);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new[] { e.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            try
            {
                var products = await context.Products.ToListAsync();
                var productDtos = mapper.Map<IEnumerable<ProductDto>>(products);
                return OperationResult<IEnumerable<ProductDto>>.Success(productDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<ProductDto>>.Failure([ e.Message ]);
            }
        }

        public async Task<OperationResult<ProductDto>> GetProductByIdAsync(int productId)
        {
            try
            {
                var product = await context.Products.FindAsync(productId);
                if (product == null) return OperationResult<ProductDto>.Failure(["Product not found."]);

                var productDto = mapper.Map<ProductDto>(product);
                return OperationResult<ProductDto>.Success(productDto);
            }
            catch (Exception e)
            {
                return OperationResult<ProductDto>.Failure([e.Message]);
            }
        }

        public async Task<OperationResult> AddItemsToManifestAsync(int manifestId, IEnumerable<ManifestItemDto> manifestItems)
        {
            try
            {
                var manifest = await context.Manifests.FindAsync(manifestId);
                if (manifest == null) return OperationResult.Failure(new[] { "Manifest not found" });

                var newManifestItems = mapper.Map<IEnumerable<ManifestItem>>(manifestItems);
                foreach (var item in newManifestItems)
                {
                    var product = await context.Products.FindAsync(item.ProductId);
                    if (product == null) return OperationResult.Failure(new[] { "Product not found" });
                    item.ManifestId = manifestId;
                }

                context.ManifestItems.AddRange(newManifestItems);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new[] { e.Message });
            }
        }

        public async Task<OperationResult> RemoveItemsFromManifestAsync(int manifestId, IEnumerable<int> itemIds)
        {
            try
            {
                var manifestItems = await context.ManifestItems
                    .Where(item => itemIds.Contains(item.ManifestItemId))
                    .ToListAsync();

                context.ManifestItems.RemoveRange(manifestItems);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new[] { e.Message });
            }
        }

        public async Task<OperationResult> UpdateManifestItemsAsync(int manifestId, IEnumerable<ManifestItemDto> manifestItems)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var manifest = await context.Manifests.FindAsync(manifestId);
                if (manifest == null) return OperationResult.Failure(new[] { "Manifest not found" });

                var existingItems = await context.ManifestItems
                    .Where(item => item.ManifestId == manifestId)
                    .ToListAsync();

                var newItems = mapper.Map<IEnumerable<ManifestItem>>(manifestItems);
                foreach (var item in newItems)
                {
                    item.ManifestId = manifestId;
                }

                context.ManifestItems.RemoveRange(existingItems);
                context.ManifestItems.AddRange(newItems);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return OperationResult.Failure(new[] { e.Message });
            }
        }
    }
}
