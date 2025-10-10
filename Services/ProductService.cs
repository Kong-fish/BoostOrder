using System.Net.Http.Headers;
using System.Text.Json;
using BO_Mobile.Models;

namespace BO_Mobile.Services;

public class ProductService
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "https://cloud.boostorder.com/bo-mart/api/v1/wp-json/wc/v1/bo/products";
    private const string Username = "ck_b9e4e281dc7aa5595062207a479090a390304335";
    private const string Password = "cs_95b5c4724a48737ed72daf8314dae9cbc83842ae";

    public ProductService()
    {
        _httpClient = new HttpClient();
        // Set up Basic Authentication
        var authToken = System.Text.Encoding.UTF8.GetBytes($"{Username}:{Password}");
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
                // This will catch API errors like authentication failures
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to fetch products. Status: {response.StatusCode}, Content: {errorContent}");
            }
        } while (currentPage <= totalPages);

        return allProducts;
    }
}