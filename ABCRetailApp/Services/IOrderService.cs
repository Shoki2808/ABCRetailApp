using ABCRetailApp.Models;

namespace ABCRetailApp.Services;

public interface IOrderService
{
    Task<IReadOnlyList<Order>> GetRecentAsync(int take = 50);
    Task<IReadOnlyList<Order>> GetForCustomerAsync(string customerId);
    Task<Order?> GetAsync(string id, string customerId);
    Task<string> CreateAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(string id, string customerId);
}
