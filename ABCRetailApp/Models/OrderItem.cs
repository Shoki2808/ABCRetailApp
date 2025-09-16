namespace ABCRetailApp.Models;

public class OrderItem
{
    public string ProductId { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal => UnitPrice * Quantity;
}
