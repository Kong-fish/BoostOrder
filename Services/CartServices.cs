using System.Collections.ObjectModel;
using BO_Mobile.Models;

namespace BO_Mobile.Services;

public class CartService
{
    public ObservableCollection<CartItem> Items { get; } = new();

    public event Action CartChanged;

    public int ItemCount => Items.Count;

    public CartService()
    {
        Items.CollectionChanged += (s, e) => NotifyStateChanged();
    }

    // --- THIS IS THE FIX: A simplified, more robust AddToCart method ---
    /// <summary>
    /// Adds a product with a specific variation to the cart. If the item already
    /// exists, its quantity is incremented by one.
    /// </summary>
    public void AddToCart(Product product, Variation selectedVariation)
    {
        if (product == null || selectedVariation == null)
            return;

        var existingItem = Items.FirstOrDefault(item => item.VariationId == selectedVariation.Id);

        if (existingItem != null)
        {
            // If the item already exists, just increase its quantity by 1.
            existingItem.Quantity++;
            NotifyStateChanged(); 
        }
        else
        {
            // If it's a new item, add it to the cart with a quantity of 1.
            decimal.TryParse(selectedVariation.RegularPrice, out var price);
            Items.Add(new CartItem
            {
                ProductId = product.Id,
                VariationId = selectedVariation.Id,
                Name = product.Name,
                Sku = selectedVariation.Sku,
                Price = price,
                ImageUrl = product.ImageUrl,
                Quantity = 1 // Always start with quantity 1
            });
            // The CollectionChanged event will automatically call NotifyStateChanged
        }
    }

    public void NotifyStateChanged()
    {
        CartChanged?.Invoke();
    }
}

