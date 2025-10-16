using System.Text.Json.Serialization;

namespace BO_Mobile.Models;
//Turn Data fetch from API into List<Product>
public class ProductResponse
{
    [JsonPropertyName("products")]
    public List<Product> Products { get; set; }
}
