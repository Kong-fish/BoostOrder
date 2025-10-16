using BO_Mobile.ViewModels;

namespace BO_Mobile.Views;

public partial class CartPage : ContentPage
{
    private readonly CartViewModel _viewModel;

	public CartPage(CartViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.UpdateCartTotals();
    }
}

