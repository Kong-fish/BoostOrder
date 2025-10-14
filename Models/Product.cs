using System.Diagnostics;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BO_Mobile.Models;

public partial class Product : ObservableObject
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

    // --- THIS IS THE FIX ---
    // Changed from QuantityToAdd and initialized to 0.
    // This property will now reflect the actual quantity of this item in the cart.
    [ObservableProperty]
    private int _quantityInCart = 0;

    [ObservableProperty]
    private Variation _selectedVariation;

    public string DisplayPrice => SelectedVariation?.RegularPrice ?? "0.00";
    
    public string ImageUrl
    {
        get
        {
            var url = Images?.FirstOrDefault()?.Src;
            Debug.WriteLine($"[Image URL] For product '{Name}': {url ?? "NULL"}");
            return url;
        }
    }

    public void SetDefaultVariation()
    {
        SelectedVariation = Variations?.FirstOrDefault();
    }
}

public class Image
{
    [JsonPropertyName("src")]
    public string Src { get; set; }
}

public class Variation
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("sku")]
    public string Sku { get; set; }

    [JsonPropertyName("regular_price")]
    public string RegularPrice { get; set; }

    [JsonPropertyName("stock_quantity")]
    public int? StockQuantity { get; set; }

    [JsonPropertyName("uom")]
    public string Uom { get; set; }
}

