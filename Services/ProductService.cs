using System.Net.Http.Headers;
using System.Text.Json;
using BO_Mobile.Models;

namespace BO_Mobile.Services;
// Fetch products from API [FULLFILLED]
public class ProductService
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "https://cloud.boostorder.com/bo-mart/api/v1/wp-json/wc/v1/bo/products";

    //Use Basic Auth when calling our endpoints. [FULLFILLED]
    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task InitializeAsync()
    {
        var username = await SecureStorage.GetAsync("ApiUsername");
        var password = await SecureStorage.GetAsync("ApiPassword");

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            throw new InvalidOperationException("API credentials not found. Ensure they are saved to SecureStorage before initialization.");
        }

        // Apply Basic Authentication
        var authToken = System.Text.Encoding.UTF8.GetBytes($"{username}:{password}");
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        var allProducts = new List<Product>();
        int currentPage = 1;
        int totalPages = 1;

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        do
        {
            var response = await _httpClient.GetAsync($"{ApiUrl}?page={currentPage}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                
                // Deserialize into the wrapper class first
                var responseData = JsonSerializer.Deserialize<ProductResponse>(jsonString, serializerOptions);

                // Then extract the list of products from the wrapper
                var productsOnPage = responseData?.Products;

                if (productsOnPage != null)
                {
                    allProducts.AddRange(productsOnPage);
                }

                if (currentPage == 1 && response.Headers.TryGetValues("X-WC-TotalPages", out var values))
                {
                    int.TryParse(values.FirstOrDefault(), out totalPages);
                }

                currentPage++;
            }
            else
            {
                // Catch authentication error
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to fetch products. Status: {response.StatusCode}, Content: {errorContent}");
            }
        } while (currentPage <= totalPages);

        return allProducts;
    }
}