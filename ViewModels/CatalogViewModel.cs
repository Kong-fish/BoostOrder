using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BO_Mobile.Models;
using BO_Mobile.Services;
using BO_Mobile.Views;

namespace BO_Mobile.ViewModels;

public partial class CatalogViewModel : ObservableObject
{
    private readonly ProductService _productService;
    private readonly DatabaseService _databaseService;
    private readonly CartService _cartService;

    [ObservableProperty]
    private ObservableCollection<Product> _products;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private int _cartItemCount;

    // to hide data when API is loading
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    public bool IsNotBusy => !IsBusy;
    private List<Product> _allProducts = new();

    public CatalogViewModel(ProductService productService, DatabaseService databaseService, CartService cartService)
    {
        _productService = productService;
        _databaseService = databaseService;
        _cartService = cartService;
        _products = new ObservableCollection<Product>();

        _cartService.CartChanged += UpdateCartBadge;
        
        GetProductsCommand.Execute(null);
        UpdateCartBadge();
    }

    private void UpdateCartBadge()
    {
        CartItemCount = _cartService.ItemCount;
    }
    
    [RelayCommand]
    private void AddToCart(Product product)
    {
        if (product == null || product.SelectedVariation == null)
            return;

        _cartService.AddToCart(product, product.SelectedVariation, product.QuantityToAdd);
        Shell.Current.DisplayAlert("Added to Cart", $"{product.QuantityToAdd} x {product.Name} ({product.SelectedVariation.Uom}) was added.", "OK");
        
        product.QuantityToAdd = 1;
    }

    [RelayCommand]
    private void IncrementQuantity(Product product)
    {
        if (product != null)
        {
            product.QuantityToAdd++;
        }
    }

    [RelayCommand]
    private void DecrementQuantity(Product product)
    {
        if (product != null && product.QuantityToAdd > 1)
        {
            product.QuantityToAdd--;
        }
    }

    [RelayCommand]
    private async Task GoToCartAsync()
    {
        await Shell.Current.GoToAsync(nameof(CartPage));
    }

    [RelayCommand]
    private async Task GetProductsAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                var apiProducts = await _productService.GetProductsAsync();
                _allProducts = apiProducts.Where(p => p.Type == "variable").ToList();
                await _databaseService.SaveProductsAsync(_allProducts);
            }
            else
            {
                _allProducts = await _databaseService.GetProductsAsync();
            }
            // Set the default variation for each product AFTER loading the data, regardless of the source (API or local database).
            _allProducts.ForEach(p => p.SetDefaultVariation());

            FilterProducts();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Unable to get products: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        FilterProducts();
    }

    // c. Product search [FULLFILLED]
    private void FilterProducts()
    {
        var filteredList = string.IsNullOrWhiteSpace(SearchText)
            ? _allProducts
            : _allProducts.Where(p =>
                p.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                p.Sku.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
        
        Products.Clear();
        foreach (var product in filteredList)
        {
            Products.Add(product);
        }
    }
}

