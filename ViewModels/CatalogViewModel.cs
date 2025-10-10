namespace BO_Mobile.ViewModels;
using BO_Mobile.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

public partial class CatalogViewModel : ObservableObject
{
    private readonly ProductService _productService;
    // You would create and inject a DatabaseService here for offline storage

    [ObservableProperty]
    private ObservableCollection<Product> _products;

    [ObservableProperty]
    private string _searchText;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCartVisible))]
    private int _cartItemCount;

    public bool IsCartVisible => CartItemCount > 0;

    public CatalogViewModel(ProductService productService)
    {
        _productService = productService;
        _products = new ObservableCollection<Product>();
    }

    [RelayCommand]
    private async Task LoadProductsAsync()
    {
        // To implement offline storage:
        // 1. Check for internet connectivity.
        // 2. If online, call _productService.GetAllProductsAsync().
        // 3. Save the results to your SQLite database.
        // 4. If offline, load from the SQLite database instead.

        var productList = await _productService.GetAllProductsAsync();
        Products = new ObservableCollection<Product>(productList);
    }

    [RelayCommand]
    private void AddToCart(Product product)
    {
        // Logic to add the product to a cart service/manager
        CartItemCount++; // This will update the badge automatically
    }

    [RelayCommand]
    private void SearchProducts()
    {
        // Logic to filter the 'Products' collection based on 'SearchText'
    }
}
