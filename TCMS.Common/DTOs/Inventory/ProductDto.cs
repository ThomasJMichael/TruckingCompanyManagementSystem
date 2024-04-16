using System.ComponentModel.DataAnnotations;

namespace TCMS.Common.DTOs.Inventory;

public class ProductDto
{
    public int ProductId { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Product name must be 100 characters or less.")]
    public string Name { get; set; }

    [StringLength(500, ErrorMessage = "Product description must be 500 characters or less.")]
    public string Description { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }
}