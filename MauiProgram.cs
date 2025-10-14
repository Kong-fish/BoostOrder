using Microsoft.Extensions.Logging;
using BO_Mobile.Services;
using BO_Mobile.ViewModels;
using BO_Mobile.Views;
using FFImageLoading.Maui; // For product image caching library

namespace BO_Mobile;

//
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseFFImageLoading() //Image Caching 
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
		// Dependency Injection: allow MVVM 
        // Services
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<ProductService>();
        builder.Services.AddSingleton<CartService>();
        
        // ViewModels
        builder.Services.AddTransient<CatalogViewModel>();
        // Changed from AddTransient to AddSingleton for the CartViewModel
        builder.Services.AddSingleton<CartViewModel>();
        // -----------------------

        // Views (Pages)
        builder.Services.AddTransient<CatalogPage>();
        builder.Services.AddTransient<CartPage>();


#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

