using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BO_Mobile.Models;
using BO_Mobile.Services;
namespace BO_Mobile.ViewModels;

public partial class CatalogViewModel : ObservableObject
{
    // Services for data access, provided by dependency injection
    private readonly ProductService _productService;
    private readonly DatabaseService _databaseService;

    // --- Observable Properties ---
    // These properties will automatically update the UI when their values change.

    [ObservableProperty]
    private ObservableCollection<Product> _products; // The list of products currently displayed on the screen.

    [ObservableProperty]
    private string _searchText; // The text bound to the search bar in the UI.

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy; // A flag to show a loading indicator.

    public bool IsNotBusy => !_isBusy;

    // This is the full list of products fetched from the source. We keep it separate
    // so we can filter it without having to call the API or database again.
    private List<Product> _allProducts = new();


    // --- Constructor ---
    public CatalogViewModel(ProductService productService, DatabaseService databaseService)
    {
        _productService = productService;
        _databaseService = databaseService;
        _products = new ObservableCollection<Product>();

        // Load the products as soon as the ViewModel is created
        GetProductsCommand.Execute(null);
    }


    // --- Relay Commands ---
    // These are the methods that will be executed when a user interacts with the UI (e.g., clicks a button).

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
                // ONLINE: Fetch from API, filter, and save to local DB
                var apiProducts = await _productService.GetProductsAsync();

                // Requirement: Only show products with type 'variable'
                _allProducts = apiProducts.Where(p => p.Type == "variable").ToList();

                // Save the fresh data for offline use
                await _databaseService.SaveProductsAsync(_allProducts);
            }
            else
            {
                // OFFLINE: Load from the local database
                _allProducts = await _databaseService.GetProductsAsync();
            }

            // Update the displayed list
            FilterProducts();
        }
        catch (Exception ex)
        {
            // Handle any errors (e.g., show a pop-up to the user)
            await Shell.Current.DisplayAlert("Error", $"Unable to get products: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    // --- Search Logic ---
    // This method is automatically called by the CommunityToolkit whenever the SearchText property changes.
    partial void OnSearchTextChanged(string value)
    {
        FilterProducts();
    }

    private void FilterProducts()
    {
        var filteredList = string.IsNullOrWhiteSpace(SearchText)
            ? _allProducts // If search is empty, show all products
            : _allProducts.Where(p =>
                p.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                p.Sku.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

        // Clear the current list and add the filtered items
        Products.Clear();
        foreach (var product in filteredList)
        {
            Products.Add(product);
        }
    }
}