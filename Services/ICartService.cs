using FurniShop.Models;

namespace FurniShop.Services;

public interface ICartService
{
    List<CartLine> GetCart();
    void AddToCart(Product product, int quantity);
    void UpdateQuantity(int productId, int quantity);
    void RemoveFromCart(int productId);
    void ClearCart();

    string? GetAppliedCouponCode();
    void ApplyCoupon(string code);
    void RemoveCoupon();
}
