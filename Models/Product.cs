using System.Diagnostics;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BO_Mobile.Models;
//Default Procudt class
//class allow complier to data bind
public partial class Product : ObservableObject
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("sku")]
    public required string Sku { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("in_stock")]
    public bool InStock { get; set; }

    [JsonPropertyName("images")]
    public required List<Image> Images { get; set; }

    [JsonPropertyName("variations")]
    public required List<Variation> Variations { get; set; }

    [ObservableProperty]
    private int _quantityToAdd = 1;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayPrice))]
    [NotifyPropertyChangedFor(nameof(DisplayStockQuantity))]
    //When change UOM or stock can update
    private Variation _selectedVariation;

    public string DisplayPrice => SelectedVariation?.RegularPrice ?? "0.00";
    public int? DisplayStockQuantity => SelectedVariation?.Inventory?.Sum(i => (int)i.StockQuantity);
    // use int here since stock is always whole number
    
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
    public required string Src { get; set; }
}
public class Inventory
{
    [JsonPropertyName("stock_quantity")]
    public decimal StockQuantity { get; set; }
}
//Different variation of the product
public class Variation
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("sku")]
    public required string Sku { get; set; }

    [JsonPropertyName("regular_price")]
    public required string RegularPrice { get; set; }

    [JsonPropertyName("inventory")]
    public List<Inventory> Inventory { get; set; }

    [JsonPropertyName("uom")]
    public required string Uom { get; set; }
}

