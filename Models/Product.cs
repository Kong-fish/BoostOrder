namespace BO_Mobile.Models;
using System.Text.Json.Serialization;

public class Product
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("sku")]
    public string Sku { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("price")]
    public string Price { get; set; }

    [JsonPropertyName("stock_quantity")]
    public int? StockQuantity { get; set; }

    [JsonPropertyName("images")]
    public List<ProductImage> Images { get; set; }

    [JsonPropertyName("variations")]
    public List<Variation> Variations { get; set; }

    public int QuantityInCart { get; set; } = 1;
}

public class ProductImage
{
    [JsonPropertyName("src")]
    public string Src { get; set; }
}

public class Variation
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("price")]
    public string Price { get; set; }

    [JsonPropertyName("attributes")]
    public List<VariationAttribute> Attributes { get; set; }
}

public class VariationAttribute
{
    [JsonPropertyName("name")]
    public string Name { get; set; } 

    [JsonPropertyName("option")]
    public string Option { get; set; } 
}
