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

    // Called every time when the page becomes visible
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Update the totals to ensure they are fresh every time the user navigates to the cart.
        _viewModel.UpdateCartTotals();
    }
}
