using System.Text.Json;
using FurniShop.Models;

namespace FurniShop.Services;

public class CartService : ICartService
{
    private const string CartSessionKey = "Cart";
    private const string CouponSessionKey = "CartCoupon";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession Session =>
        _httpContextAccessor.HttpContext?.Session
        ?? throw new InvalidOperationException("Session is not available. Make sure app.UseSession() runs before this is used.");

    public List<CartLine> GetCart()
    {
        var json = Session.GetString(CartSessionKey);
        if (string.IsNullOrEmpty(json))
            return new List<CartLine>();

        return JsonSerializer.Deserialize<List<CartLine>>(json) ?? new List<CartLine>();
    }

    private void SaveCart(List<CartLine> cart)
    {
        Session.SetString(CartSessionKey, JsonSerializer.Serialize(cart));
    }

    public void AddToCart(Product product, int quantity)
    {
        if (quantity < 1) quantity = 1;

        var cart = GetCart();
        var existing = cart.FirstOrDefault(l => l.ProductId == product.Id);

        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            cart.Add(new CartLine
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Quantity = quantity
            });
        }

        SaveCart(cart);
    }

    public void UpdateQuantity(int productId, int quantity)
    {
        var cart = GetCart();
        var line = cart.FirstOrDefault(l => l.ProductId == productId);
        if (line == null) return;

        if (quantity < 1)
        {
            cart.Remove(line);
        }
        else
        {
            line.Quantity = quantity;
        }

        SaveCart(cart);
    }

    public void RemoveFromCart(int productId)
    {
        var cart = GetCart();
        cart.RemoveAll(l => l.ProductId == productId);
        SaveCart(cart);
    }

    public void ClearCart()
    {
        Session.Remove(CartSessionKey);
        Session.Remove(CouponSessionKey);
    }

    public string? GetAppliedCouponCode() => Session.GetString(CouponSessionKey);

    public void ApplyCoupon(string code) => Session.SetString(CouponSessionKey, code);

    public void RemoveCoupon() => Session.Remove(CouponSessionKey);
}
