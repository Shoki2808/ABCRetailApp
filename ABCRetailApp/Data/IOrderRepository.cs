using ABCRetailApp.Models;

namespace ABCRetailApp.Data;

public interface IOrderRepository
{
    Task InitializeAsync();
    Task<Order?> GetAsync(string id, string partitionKey);
    Task<IReadOnlyList<Order>> ListByCustomerAsync(string customerId);
    Task<IReadOnlyList<Order>> ListRecentAsync(int take = 50);
    Task CreateAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(string id, string partitionKey);
}
