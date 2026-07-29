using FurniShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurniShop.Controllers;

public class ShopController : Controller
{
    private readonly ApplicationDbContext _db;

    public ShopController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET /Shop
    public async Task<IActionResult> Index(string? category)
    {
        var query = _db.Products.Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(p => p.Category == category);

        var products = await query.OrderBy(p => p.Name).ToListAsync();

        ViewBag.SelectedCategory = category;
        ViewBag.Categories = await _db.Products
            .Where(p => p.IsActive && p.Category != null)
            .Select(p => p.Category!)
            .Distinct()
            .ToListAsync();

        return View(products);
    }
}
