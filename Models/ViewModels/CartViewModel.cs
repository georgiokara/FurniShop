namespace FurniShop.Models.ViewModels;

public class CartViewModel
{
    public List<CartLine> Lines { get; set; } = new();
    public string? AppliedCouponCode { get; set; }
    public int DiscountPercent { get; set; }
    public string? CouponMessage { get; set; }

    public decimal Subtotal => Lines.Sum(l => l.LineTotal);
    public decimal DiscountAmount => Subtotal * DiscountPercent / 100m;
    public decimal Total => Subtotal - DiscountAmount;
}
