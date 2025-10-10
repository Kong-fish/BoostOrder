using System.Text.Json.Serialization;

namespace BO_Mobile.Models;

public class ProductResponse
{
    [JsonPropertyName("products")]
    public List<Product> Products { get; set; }
}
