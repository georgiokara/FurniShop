using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniShop.Models;

public class Product
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    // Relative path under wwwroot/images, e.g. "images/product-1.png"
    [Required, StringLength(300)]
    public string ImageUrl { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Category { get; set; }

    public int StockQuantity { get; set; } = 100;

    public bool IsActive { get; set; } = true;
}
