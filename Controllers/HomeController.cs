using FurniShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurniShop.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var featured = await _db.Products
            .Where(p => p.IsActive)
            .OrderBy(p => p.Id)
            .Take(3)
            .ToListAsync();

        return View(featured);
    }

    public IActionResult Error() => View();
}
