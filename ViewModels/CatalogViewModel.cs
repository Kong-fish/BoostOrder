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
    private string _searchText = string.Empty; // FIX: Initialize to a non-null value

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

        GetProductsCommand.Execute(null);
    }

    [RelayCommand]
    private void AddToCart(Product product)
    {
        if (product == null)
            return;

        _cartService.AddToCart(product);
    }

    [RelayCommand]
    private async Task GoToCartAsync()
    {
        await Shell.Current.GoToAsync(nameof(CartPage));
    }

    [RelayCommand]
    private async Task GetProductsAsync()
    {
        if (IsBusy)
            return;
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

