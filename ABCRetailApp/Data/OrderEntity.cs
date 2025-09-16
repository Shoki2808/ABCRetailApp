using ABCRetailApp.Models;
using Azure;
using Azure.Data.Tables;

namespace ABCRetailApp.Data
{
    public class OrderEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = string.Empty; // customerId
        public string RowKey { get; set; } = string.Empty;       // orderId
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Extra properties matching your Order model
        public DateTime CreatedUtc { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }

        public static OrderEntity FromOrder(Order order) => new OrderEntity
        {
            PartitionKey = order.CustomerId,
            RowKey = order.Id,
            CreatedUtc = order.CreatedUtc,
            ProductName = order.ProductName,
            Quantity = order.Quantity
        };

        public Order ToOrder() => new Order
        {
            Id = RowKey,
            CustomerId = PartitionKey,
            CreatedUtc = CreatedUtc,
            ProductName = ProductName ?? string.Empty,
            Quantity = Quantity
        };
    }
}
