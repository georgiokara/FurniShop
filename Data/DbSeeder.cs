using FurniShop.Models;

namespace FurniShop.Data;

public static class DbSeeder
{
    public static void Seed(ApplicationDbContext db)
    {
        if (!db.Products.Any())
        {
            db.Products.AddRange(
                new Product { Name = "Nordic Chair", Description = "Minimalist wooden chair with a soft cushioned seat.", Price = 50.00m, ImageUrl = "images/product-3.png", Category = "Chairs" },
                new Product { Name = "Kruzo Aero Chair", Description = "Ergonomic mesh-back office chair.", Price = 78.00m, ImageUrl = "images/product-2.png", Category = "Chairs" },
                new Product { Name = "Ergonomic Chair", Description = "Adjustable lumbar-support desk chair.", Price = 43.00m, ImageUrl = "images/product-1.png", Category = "Chairs" },
                new Product { Name = "Classic Sofa", Description = "Three-seater sofa in soft grey fabric.", Price = 320.00m, ImageUrl = "images/sofa.png", Category = "Sofas" },
                new Product { Name = "Lounge Couch", Description = "Low-profile couch, great for small living rooms.", Price = 275.00m, ImageUrl = "images/couch.png", Category = "Sofas" },
                new Product { Name = "Ceramic Bowl Set", Description = "Hand-finished decorative bowl set.", Price = 22.00m, ImageUrl = "images/bowl-2.png", Category = "Decor" },
                new Product { Name = "Wooden Bowl", Description = "Solid oak decorative bowl.", Price = 18.00m, ImageUrl = "images/bowl-3.png", Category = "Decor" }
            );
        }

        if (!db.Coupons.Any())
        {
            db.Coupons.AddRange(
                new Coupon { Code = "WELCOME10", DiscountPercent = 10, IsActive = true },
                new Coupon { Code = "FURNI20", DiscountPercent = 20, IsActive = true, ExpiresOn = DateTime.UtcNow.AddMonths(3) }
            );
        }

        db.SaveChanges();
    }
}
