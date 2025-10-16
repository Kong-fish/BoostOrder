using System.Collections.ObjectModel;
using BO_Mobile.Models;
using BO_Mobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BO_Mobile.ViewModels;

public partial class CartViewModel : ObservableObject
{
    private readonly CartService _cartService;

    // Exposes the list of items directly from the cart service
    public ObservableCollection<CartItem> Items => _cartService.Items;

    [ObservableProperty]
    private int _totalItemCount;

    [ObservableProperty]
    private decimal _cartTotal;

    public CartViewModel(CartService cartService)
    {
        _cartService = cartService;
        // Subscribe to changes in the cart to keep totals updated in real-time
        _cartService.Items.CollectionChanged += (s, e) => UpdateCartTotals();
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        // Navigates back to the previous page (the catalog)
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private void IncreaseQuantity(CartItem item)
    {
        if (item == null) return;
        item.Quantity++;
        UpdateCartTotals();
    }

    [RelayCommand]
    private void DecreaseQuantity(CartItem item)
    {
        if (item == null) return;
        
        if (item.Quantity > 1)
        {
            item.Quantity--;
        }
        else
        {
            // If quantity is 1, decreasing it removes the item.
            RemoveItem(item);
        }
        UpdateCartTotals();
    }

    [RelayCommand]
    private void RemoveItem(CartItem item)
    {
        if (item == null) return;
        Items.Remove(item);
        // UpdateCartTotals is automatically called by the CollectionChanged event.
    }

    [RelayCommand]
    private async Task ClearCart()
    {
        bool confirm = await Shell.Current.DisplayAlert("Clear Cart", "Are you sure you want to remove all items from your cart?", "Yes", "No");
        if (confirm)
        {
            Items.Clear();
            // UpdateCartTotals is automatically called by the CollectionChanged event.
        }
    }

    // Calculates the totals and notifies the CartService to update the badge.
    public void UpdateCartTotals()
    {
        TotalItemCount = Items.Sum(i => i.Quantity);
        CartTotal = Items.Sum(i => i.Total);
        
        // Explicitly tell the CartService to notify subscribers (like the CatalogViewModel)
        // that a change has occurred, so the badge can update.
        _cartService.NotifyStateChanged();
    }
}

