using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniShop.Models;

public class Coupon
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string Code { get; set; } = string.Empty;

    // Percentage discount, e.g. 15 = 15% off. Keep it simple: percentage only.
    [Range(1, 100)]
    public int DiscountPercent { get; set; }

    public DateTime? ExpiresOn { get; set; }

    public bool IsActive { get; set; } = true;
}
