using BO_Mobile.Views; 
namespace BO_Mobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(CartPage), typeof(CartPage));
	}
}
