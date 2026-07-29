using System.ComponentModel.DataAnnotations;

namespace FurniShop.Models.ViewModels;

public class CheckoutViewModel
{
    [Required, Display(Name = "Full name")]
    public string ShippingName { get; set; } = string.Empty;

    [Required, Display(Name = "Address")]
    public string ShippingAddress { get; set; } = string.Empty;

    [Required, Display(Name = "City")]
    public string ShippingCity { get; set; } = string.Empty;

    [Required, Phone, Display(Name = "Phone")]
    public string ShippingPhone { get; set; } = string.Empty;

    // Populated from session for display only, not posted back
    public CartViewModel Cart { get; set; } = new();
}
