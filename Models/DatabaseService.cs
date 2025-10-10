namespace BO_Mobile.Models;
using SQLite;
using System.Text.Json;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;
    private bool _isInitialized = false;

    // This defines a path for the database file on the device
    private static string DbPath => Path.Combine(FileSystem.AppDataDirectory, "products.db3");

    public DatabaseService()
    {
        _database = new SQLiteAsyncConnection(DbPath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache);
    }

    private async Task InitializeAsync()
    {
        if (_isInitialized)
            return;

        // Create the Product table if it doesn't exist
        await _database.CreateTableAsync<ProductDto>();
        _isInitialized = true;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        await InitializeAsync();
        var productDtos = await _database.Table<ProductDto>().ToListAsync();

        // Convert from DTO back to the main Product model
        return productDtos.Select(dto => dto.ToProduct()).ToList();
    }

    public async Task SaveProductsAsync(List<Product> products)
    {
        await InitializeAsync();

        // Convert products to DTOs for safe database storage
        var productDtos = products.Select(p => new ProductDto(p)).ToList();

        // Clear the old data and insert the new fresh data
        await _database.DeleteAllAsync<ProductDto>();
        await _database.InsertAllAsync(productDtos);
    }
}

// A "Data Transfer Object" for storing Products in SQLite.
// SQLite can't store complex objects like lists directly, so we serialize them to JSON strings.
public class ProductDto
{
    [PrimaryKey]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Sku { get; set; }
    public string Type { get; set; }
    public string Price { get; set; }
    public int? StockQuantity { get; set; }

    // Store complex data as a JSON string
    public string ImagesJson { get; set; }
    public string VariationsJson { get; set; }

    public ProductDto() { }

    // Constructor to convert from Product to ProductDto
    public ProductDto(Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Sku = product.Sku;
        Type = product.Type;
        Price = product.Price;
        StockQuantity = product.StockQuantity;
        ImagesJson = JsonSerializer.Serialize(product.Images);
        VariationsJson = JsonSerializer.Serialize(product.Variations);
    }

    // Method to convert from ProductDto back to Product
    public Product ToProduct()
    {
        return new Product
        {
            Id = this.Id,
            Name = this.Name,
            Sku = this.Sku,
            Type = this.Type,
            Price = this.Price,
            StockQuantity = this.StockQuantity,
            Images = JsonSerializer.Deserialize<List<ProductImage>>(this.ImagesJson),
            Variations = JsonSerializer.Deserialize<List<Variation>>(this.VariationsJson)
        };
    }
}
