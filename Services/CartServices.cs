using System.Collections.ObjectModel;
using BO_Mobile.Models;

namespace BO_Mobile.Services;

public class CartService
{
    public ObservableCollection<CartItem> Items { get; } = new();

    // This event is what the CatalogViewModel subscribes to.
    // When this event is triggered, the catalog page knows to update the badge.
    public event Action CartChanged;

    // A property to easily calculate the number of unique line items in the cart.
    public int ItemCount => Items.Count;

    public CartService()
    {
        // When an item is added or removed from the list, notify subscribers.
        Items.CollectionChanged += (s, e) => NotifyStateChanged();
    }

    /// <summary>
    /// Adds a product with a specific variation and quantity to the shopping cart.
    /// </summary>
    public void AddToCart(Product product, Variation selectedVariation, int quantity)
    {
        if (product == null || selectedVariation == null || quantity <= 0)
            return;

        var existingItem = Items.FirstOrDefault(item => item.VariationId == selectedVariation.Id);

        if (existingItem != null)
        {
            // If the item already exists, increase its quantity by the specified amount.
            existingItem.Quantity += quantity;
            // Manually notify because a property of an item changed,
            // which the CollectionChanged event doesn't catch.
            NotifyStateChanged(); 
        }
        else
        {
            // If it's a new item, create a new CartItem and add it to the list.
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
                Uom = selectedVariation.Uom // Populate the Uom property
            });
            // The CollectionChanged event will automatically call NotifyStateChanged.
        }
    }

    // This method is called to trigger the CartChanged event.
    public void NotifyStateChanged()
    {
        CartChanged?.Invoke();
    }
}

