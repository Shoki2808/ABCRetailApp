namespace ABCRetailApp.Models;

public class Product
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ProductCategory { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? StockQuantity { get; set; }
    public bool IsActive { get; set; }


}
