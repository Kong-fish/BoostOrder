using System.Collections.ObjectModel;
using BO_Mobile.Models;

namespace BO_Mobile.Services;

public class CartService
{
    // The list of items in the cart. ObservableCollection will notify the UI of changes.
    public ObservableCollection<CartItem> Items { get; } = new();

    // A property to easily get the total number of items for the badge
    public int ItemCount => Items.Sum(item => item.Quantity);

    public void AddToCart(Product product)
    {
        // Find if the item already exists in the cart
        var existingItem = Items.FirstOrDefault(item => item.ProductId == product.Id);

        if (existingItem != null)
        {
            // If it exists, just increase the quantity
            existingItem.Quantity++;
        }
        else
        {
            // If it's a new item, add it to the cart
            decimal.TryParse(product.DisplayPrice, out var price);
            Items.Add(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                Price = price,
                ImageUrl = product.ImageUrl,
                Quantity = 1
            });
        }
    }
}

