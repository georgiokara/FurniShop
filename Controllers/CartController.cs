using FurniShop.Data;
using FurniShop.Models.ViewModels;
using FurniShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace FurniShop.Controllers;

public class CartController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ICartService _cart;

    public CartController(ApplicationDbContext db, ICartService cart)
    {
        _db = db;
        _cart = cart;
    }

    // GET /Cart
    public IActionResult Index()
    {
        return View(BuildViewModel());
    }

    // POST /Cart/Add
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(int productId, int quantity = 1)
    {
        var product = _db.Products.Find(productId);
        if (product == null) return NotFound();

        _cart.AddToCart(product, quantity);
        return RedirectToAction(nameof(Index));
    }

    // POST /Cart/UpdateQuantity
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateQuantity(int productId, int quantity)
    {
        _cart.UpdateQuantity(productId, quantity);
        return RedirectToAction(nameof(Index));
    }

    // POST /Cart/Remove
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(int productId)
    {
        _cart.RemoveFromCart(productId);
        return RedirectToAction(nameof(Index));
    }

    // POST /Cart/ApplyCoupon
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ApplyCoupon(string couponCode)
    {
        var code = (couponCode ?? string.Empty).Trim().ToUpperInvariant();
        var coupon = _db.Coupons.FirstOrDefault(c => c.Code.ToUpper() == code);

        var vm = BuildViewModel();

        if (coupon == null || !coupon.IsActive || (coupon.ExpiresOn.HasValue && coupon.ExpiresOn < DateTime.UtcNow))
        {
            _cart.RemoveCoupon();
            TempData["CouponMessage"] = "That coupon code is invalid or has expired.";
        }
        else
        {
            _cart.ApplyCoupon(coupon.Code);
            TempData["CouponMessage"] = $"Coupon \"{coupon.Code}\" applied: {coupon.DiscountPercent}% off.";
        }

        return RedirectToAction(nameof(Index));
    }

    private CartViewModel BuildViewModel()
    {
        var lines = _cart.GetCart();
        var appliedCode = _cart.GetAppliedCouponCode();

        var vm = new CartViewModel
        {
            Lines = lines,
            AppliedCouponCode = appliedCode,
            CouponMessage = TempData["CouponMessage"] as string
        };

        if (!string.IsNullOrEmpty(appliedCode))
        {
            var coupon = _db.Coupons.FirstOrDefault(c => c.Code == appliedCode && c.IsActive);
            vm.DiscountPercent = coupon?.DiscountPercent ?? 0;
        }

        return vm;
    }
}
