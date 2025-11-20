using BO_Mobile.Helpers;
using BO_Mobile.Services;
namespace BO_Mobile;
public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        _ = RunInitialSetupAsync(); 
    }

    // New method to handle async setup
    private async Task RunInitialSetupAsync()
    {
        // 1. Load the secrets from local-secrets.json (only in dev, and only if not saved yet)
        // and store them securely in SecureStorage.
        await ConfigurationHelper.LoadAndSaveSecretsAsync();
        
        // 2. Initialize critical services using the ServiceProvider.
        // This is necessary because the constructor is not async.
        // Retrieve the service instance from the DI container
        var productService = Current.Handler.MauiContext.Services.GetService<ProductService>();
        
        if (productService != null)
        {
            // This call retrieves the credentials from SecureStorage and sets the 
            // Basic Auth header on the HttpClient.
            await productService.InitializeAsync();
            System.Diagnostics.Debug.WriteLine("ProductService initialized with secure credentials.");
        }
        
        // Set the main page once initial setup is complete (or let MAUI handle it)
        MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // The AppShell is now initialized in the constructor/setup logic.
        return new Window(new AppShell());
    }
}