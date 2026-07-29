using System.Security.Claims;
using FurniShop.Data;
using FurniShop.Models;
using FurniShop.Models.ViewModels;
using FurniShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurniShop.Controllers;

[Authorize] // must be signed in to check out
public class CheckoutController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ICartService _cart;

    public CheckoutController(ApplicationDbContext db, ICartService cart)
    {
        _db = db;
        _cart = cart;
    }

    // GET /Checkout
    public IActionResult Index()
    {
        var lines = _cart.GetCart();
        if (lines.Count == 0)
            return RedirectToAction("Index", "Cart");

        var vm = new CheckoutViewModel { Cart = BuildCartViewModel() };
        return View(vm);
    }

    // POST /Checkout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(CheckoutViewModel vm)
    {
        var lines = _cart.GetCart();
        if (lines.Count == 0)
            return RedirectToAction("Index", "Cart");

        if (!ModelState.IsValid)
        {
            vm.Cart = BuildCartViewModel();
            return View(vm);
        }

        var cartVm = BuildCartViewModel();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var order = new Order
        {
            UserId = userId,
            ShippingName = vm.ShippingName,
            ShippingAddress = vm.ShippingAddress,
            ShippingCity = vm.ShippingCity,
            ShippingPhone = vm.ShippingPhone,
            CouponCode = cartVm.AppliedCouponCode,
            Subtotal = cartVm.Subtotal,
            DiscountAmount = cartVm.DiscountAmount,
            Total = cartVm.Total,
            Items = lines.Select(l => new OrderItem
            {
                ProductId = l.ProductId,
                ProductName = l.ProductName,
                UnitPrice = l.Price,
                Quantity = l.Quantity
            }).ToList()
        };

        _db.Orders.Add(order);
        _db.SaveChanges();

        _cart.ClearCart();

        return RedirectToAction(nameof(ThankYou), new { id = order.Id });
    }

    public IActionResult ThankYou(int id)
    {
        var order = _db.Orders.Find(id);
        if (order == null || order.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            return NotFound();

        return View(order);
    }

    private CartViewModel BuildCartViewModel()
    {
        var lines = _cart.GetCart();
        var appliedCode = _cart.GetAppliedCouponCode();

        var vm = new CartViewModel { Lines = lines, AppliedCouponCode = appliedCode };

        if (!string.IsNullOrEmpty(appliedCode))
        {
            var coupon = _db.Coupons.FirstOrDefault(c => c.Code == appliedCode && c.IsActive);
            vm.DiscountPercent = coupon?.DiscountPercent ?? 0;
        }

        return vm;
    }
}
