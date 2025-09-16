using ABCRetailApp.Models;

namespace ABCRetailApp.Data
{
    public interface IProductRepository
    {
        Task<List<Product>> ListRecentAsync(int take = 50);
    }
}
