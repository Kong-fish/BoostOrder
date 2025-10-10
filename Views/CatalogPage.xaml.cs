namespace BO_Mobile.Views;
using BO_Mobile.ViewModels;

public partial class CatalogPage : ContentPage
{
	// This constructor takes the ViewModel and sets it as the page's data source.
	public CatalogPage(CatalogViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}