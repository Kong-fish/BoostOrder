using System.Text.Json.Serialization;

namespace BO_Mobile.Models;
//list of product from api
//then got to product to seperate
public class ProductResponse
{
    [JsonPropertyName("products")]
    public List<Product> Products { get; set; }
}
