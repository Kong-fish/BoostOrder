using System.Collections.ObjectModel;
using BO_Mobile.Models;
using BO_Mobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BO_Mobile.ViewModels;

public partial class CartViewModel : ObservableObject
{
    private readonly CartService _cartService;

    public ObservableCollection<CartItem> Items => _cartService.Items;

    [ObservableProperty]
    private int _totalItemCount;

    [ObservableProperty]
    private decimal _cartTotal;

    public CartViewModel(CartService cartService)
    {
        _cartService = cartService;
        _cartService.Items.CollectionChanged += (s, e) => UpdateCartTotals();
        
        // Also update totals when a property (like Quantity) on an item changes
        foreach(var item in _cartService.Items)
        {
            item.PropertyChanged += (s, e) => UpdateCartTotals();
        }
    }

    // --- FIX: Replaced UpdateQuantity with two separate commands ---

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
        item.Quantity--;

        if (item.Quantity <= 0)
        {
            RemoveItem(item);
        }
        UpdateCartTotals();
    }

    [RelayCommand]
    private void RemoveItem(CartItem item)
    {
        if (item == null) return;
        Items.Remove(item);
        // No need to call UpdateCartTotals here, the CollectionChanged event will handle it.
    }

    [RelayCommand]
    private async Task ClearCart()
    {
        bool confirm = await Shell.Current.DisplayAlert("Clear Cart", "Are you sure you want to remove all items from your cart?", "Yes", "No");
        if (confirm)
        {
            Items.Clear();
            // No need to call UpdateCartTotals here, the CollectionChanged event will handle it.
        }
    }

    public void UpdateCartTotals()
    {
        TotalItemCount = Items.Sum(i => i.Quantity);
        CartTotal = Items.Sum(i => i.Total);
    }
}

