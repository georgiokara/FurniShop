# FurniShop — setup & deploy

## What's in here
- **Identity** — registration/login via `AddDefaultIdentity`. Register/Login/Manage pages come
  from the `Microsoft.AspNetCore.Identity.UI` package automatically (no Razor Pages files needed
  unless you want to restyle them — see below).
- **Products table** — `Product` entity, `ShopController` reads from the DB, `DbSeeder` seeds 7
  sample products from the Furni template images on first run.
- **Cart** — session-based (`ICartService`/`CartService`), no DB writes until checkout.
- **Coupons** — `Coupon` entity, seeded with `WELCOME10` (10%) and `FURNI20` (20%). Applied in
  `CartController.ApplyCoupon` and persisted onto the `Order` at checkout.
- **Orders** — created in `CheckoutController` when a signed-in user checks out; cart is cleared
  after.

## 1. Open in Visual Studio
1. Copy this `FurniShop` folder into your solution (or open the folder directly).
2. Restore NuGet packages. If the exact package versions in `FurniShop.csproj` don't resolve,
   right-click the project → **Manage NuGet Packages** → update the four Identity/EF Core packages
   to whatever matches your installed .NET 10 SDK.

## 2. Create the Azure SQL database directly (skipping local SSMS, as you asked)
1. Azure Portal → **Create a resource** → **SQL Database**.
   - Create a new **SQL server** if you don't have one — set an admin username/password here.
   - Enable **Allow Azure services to access this server** in the server's networking settings, and
     add your own IP under firewall rules so you can connect from Visual Studio/SSMS while testing.
2. Grab the connection string: SQL Database → **Connection strings** → ADO.NET tab.

## 3. Set the connection string locally — never in appsettings.json
Given the leak on SchoolApplication, treat this the same way from day one:

```
cd FurniShop
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=tcp:yourserver.database.windows.net,1433;Initial Catalog=FurniShopDb;User ID=youradmin;Password=yourpassword;Encrypt=True;"
```

`appsettings.json` in this project only has a placeholder — that's intentional, keep it that way and
let user-secrets (locally) and Azure App Service Configuration (in production) hold the real value.

## 4. First run creates the schema
`Program.cs` already calls `db.Database.Migrate()` and seeds sample data on startup, so you don't
need to run `Update-Database` by hand — just add an initial migration once:

```
dotnet ef migrations add InitialCreate
```

Then hit F5. It'll apply the migration straight to the Azure SQL database and seed products/coupons.

## 5. Deploy to Azure App Service
Since you've already got GitHub Actions/OIDC working for SchoolApplication, the same pattern applies:
1. Create an Azure **App Service** (Linux, .NET 10 stack).
2. In the App Service → **Configuration** → **Connection strings**, add `DefaultConnection` there
   (type: SQLAzure) with the real value — this is what replaces user-secrets in production.
3. Push this project to a **new** GitHub repo, or a fresh folder in an existing one — just double
   check `.gitignore` (included) is in place before your first commit so `bin/`, `obj/`, and any
   local secrets files never get committed.
4. Use the same federated-identity GitHub Actions workflow you already have working, pointed at
   this app's publish profile.

## Restyling Identity's Register/Login pages (optional)
Right now Register/Login use the default unstyled Identity UI pages. If you want them to match the
Furni look, in Visual Studio: right-click the project → **Add** → **New Scaffolded Item** →
**Identity** → select `Account\Login` and `Account\Register` → this drops editable `.cshtml` files
into `Areas/Identity/Pages/Account/` that you can restyle with the Furni CSS classes.

## Not converted yet
`about.html`, `services.html`, `blog.html`, `contact.html` are still static — they weren't part of
what you asked for here (auth, products, cart, coupons), but they're straightforward to turn into
more `HomeController` actions + Views later using the same pattern as `Views/Home/Index.cshtml`.
