using ABCRetailApp.Models;
using Azure;
using Azure.Data.Tables;

namespace ABCRetailApp.Data
{
    public class ProductEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = string.Empty; // categoryId
        public string RowKey { get; set; } = string.Empty;       // productId
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Extra properties matching your Order model
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? StockQuantity { get; set; }
        public bool IsActive { get; set; }

        public static ProductEntity FromProduct(Product product) => new ProductEntity
        {
            PartitionKey = product.ProductCategory,
            RowKey = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price, 
            ImageUrl = product.ImageUrl, 
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive
        };

        public Product ToProduct() => new Product
        {
            Id = RowKey,
            ProductCategory = PartitionKey,
            Name = Name ?? string.Empty,
            Description = Description,
            Price = Price,
            ImageUrl = ImageUrl,
            StockQuantity = StockQuantity,
            IsActive = IsActive
        };
    }
}
