namespace FurniShop.Models;

// A single line in the shopping cart. The whole cart is a List<CartLine>
// serialized to JSON and stored in the session cookie - no DB table needed
// for guests browsing. It only touches the database at checkout time,
// when it becomes an Order + OrderItems.
public class CartLine
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public decimal LineTotal => Price * Quantity;
}
