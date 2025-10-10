using System.Text.Json.Serialization;

namespace BO_Mobile.Models;

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

    [JsonPropertyName("in_stock")]
    public bool InStock { get; set; }

    [JsonPropertyName("images")]
    public List<Image> Images { get; set; }

    [JsonPropertyName("variations")]
    public List<Variation> Variations { get; set; }

    // --- Helper Properties for easy UI Binding ---

    // This gets the price from the first variation, which is what we want to display.
    public string DisplayPrice => Variations?.FirstOrDefault()?.RegularPrice ?? "0.00";

    // This gets the URL from the first image in the list.
    public string ImageUrl => Images?.FirstOrDefault()?.Src;
}

// Helper class to match the "images" array in the JSON
public class Image
{
    [JsonPropertyName("src")]
    public string Src { get; set; }
}

// Helper class to match the "variations" array in the JSON
public class Variation
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("regular_price")]
    public string RegularPrice { get; set; }

    [JsonPropertyName("stock_quantity")]
    public int? StockQuantity { get; set; }
}

