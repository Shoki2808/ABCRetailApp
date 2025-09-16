

using ABCRetailApp.Data;
using ABCRetailApp.Models;
using ABCRetailApp.Services;

namespace ABCRetailApp.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repo;

    public OrderService(IOrderRepository repo)
    {
        _repo = repo;
    }

    public Task<IReadOnlyList<Order>> GetRecentAsync(int take = 50) => _repo.ListRecentAsync(take);
    public Task<IReadOnlyList<Order>> GetForCustomerAsync(string customerId) => _repo.ListByCustomerAsync(customerId);
    public Task<Order?> GetAsync(string id, string customerId) => _repo.GetAsync(id, customerId);

    public async Task<string> CreateAsync(Order order)
    {
        order.CreatedUtc = DateTime.UtcNow;
        await _repo.CreateAsync(order);
        return order.Id;
    }

    public Task UpdateAsync(Order order) => _repo.UpdateAsync(order);
    public Task DeleteAsync(string id, string customerId) => _repo.DeleteAsync(id, customerId);
}
