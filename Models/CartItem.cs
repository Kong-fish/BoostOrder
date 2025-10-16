using CommunityToolkit.Mvvm.ComponentModel;

namespace BO_Mobile.Models;
public partial class CartItem : ObservableObject
{
    public required int ProductId { get; set; }
    public required int VariationId { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required string Sku { get; set; } = string.Empty;

    public string Uom { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }

    [ObservableProperty]
    private int _quantity;

    // Calculated property for the total price of this line item
    public decimal Total => Price * Quantity;

}