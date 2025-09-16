using Newtonsoft.Json;

namespace ABCRetailApp.Models;

public class Order
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public List<OrderItem> Items { get; set; } = new();
    public string? Notes { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }

    public decimal Total => Items.Sum(i => i.LineTotal);
}
