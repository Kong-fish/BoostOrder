namespace BO_Mobile.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class ProductService
{
    private readonly HttpClient _httpClient = new();
    private const string ApiUrl = "https://cloud.boostorder.com/bo-mart/api/v1/wp-json/wc/v1/bo/products";
    private const string Username = "ck_b9e4e281dc7aa5595062207a479090a390304335";
    private const string Password = "cs_95b5c4724a48737ed72daf8314dae9cbc83842ae";

    public ProductService()
    {
        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Username}:{Password}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        var allProducts = new List<Product>();
        var currentPage = 1;
        var totalPages = 1; // Start with 1, update after first call

        do
        {
            var response = await _httpClient.GetAsync($"{ApiUrl}?page={currentPage}");
            if (response.IsSuccessStatusCode)
            {
                // Get total pages from header
                if (currentPage == 1 && response.Headers.TryGetValues("X-WC-TotalPages", out var values))
                {
                    int.TryParse(values.FirstOrDefault(), out totalPages);
                }

                var content = await response.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<List<Product>>(content);

                if (products != null)
                {
                    // Filter for 'variable' products as required
                    allProducts.AddRange(products.Where(p => p.Type == "variable"));
                }
                currentPage++;
            }
            else
            {
                // Handle error or break loop
                break;
            }
        } while (currentPage <= totalPages);

        return allProducts;
    }
}
