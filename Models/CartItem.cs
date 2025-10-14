using CommunityToolkit.Mvvm.ComponentModel;

namespace BO_Mobile.Models;
public partial class CartItem : ObservableObject
{
    public int ProductId { get; set; }
    public string Sku { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }

    [ObservableProperty]
    private int _quantity;

    // Calculated property for the total price of this line item
    public decimal Total => Price * Quantity;

}