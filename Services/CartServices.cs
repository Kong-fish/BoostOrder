using System.Collections.ObjectModel;
using BO_Mobile.Models;

namespace BO_Mobile.Services;
// a. Badge at the cart icon – showing the number of line items added to cart
//i. It should increase as the user adds to cart [FULLFILLED]
//ii. It should decrease as the user removes from cart [FULLFILLED]
public class CartService
{
    public ObservableCollection<CartItem> Items { get; } = new();

    // Catalog page update the badge.
    public event Action CartChanged;
    public int ItemCount => Items.Count;

    public CartService()
    {
        Items.CollectionChanged += (s, e) => NotifyStateChanged();
    }

    public void AddToCart(Product product, Variation selectedVariation, int quantity)
    {
        if (product == null || selectedVariation == null || quantity <= 0)
            return;

        var existingItem = Items.FirstOrDefault(item => item.VariationId == selectedVariation.Id);

        if (existingItem != null)
        {
            // If the item already exists, increase its quantity by the specified amount.
            existingItem.Quantity += quantity;
            // Manually notify as the property of an item changed, CollectionChanged event doesn't catch.
            NotifyStateChanged(); 
        }
        else
        {
            // create a new CartItem and add it to the list.
            decimal.TryParse(selectedVariation.RegularPrice, out var price);
            Items.Add(new CartItem
            {
                ProductId = product.Id,
                VariationId = selectedVariation.Id,
                Name = product.Name,
                Sku = selectedVariation.Sku,
                Price = price,
                ImageUrl = product.ImageUrl,
                Quantity = quantity,
                Uom = selectedVariation.Uom
            });
            // The CollectionChanged event will automatically call NotifyStateChanged.
        }
    }

    // to trigger the CartChanged event.
    public void NotifyStateChanged()
    {
        CartChanged?.Invoke();
    }
}

