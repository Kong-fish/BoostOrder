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

    // This method is called every time the page becomes visible to the user.
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // We explicitly update the totals to ensure they are fresh
        // every time the user navigates to the cart.
        _viewModel.UpdateCartTotals();
    }
}
