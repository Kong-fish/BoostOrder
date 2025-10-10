using Microsoft.Extensions.Logging;
using BO_Mobile.Models;
using BO_Mobile.ViewModels;
using BO_Mobile.Views;
using BO_Mobile.Services;


namespace BO_Mobile;
//App Startup and configuration
public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<ProductService>();
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddTransient<CatalogViewModel>();
        builder.Services.AddTransient<CatalogPage>();	

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
