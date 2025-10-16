namespace BO_Mobile.Views;
using BO_Mobile.ViewModels;

public partial class CatalogPage : ContentPage
{
	private readonly CatalogViewModel _viewModel;
	// This constructor takes the ViewModel and sets it as the page's data source.
	public CatalogPage(CatalogViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Call the initialization command to load data only once when the page appears.
        await _viewModel.InitializeCommand.ExecuteAsync(null);
    }
}