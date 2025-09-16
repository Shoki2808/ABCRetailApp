using ABCRetailApp.Models;

namespace ABCRetailApp.Services
{
    public interface IProductService
    {
        //Task<List<Product>> GetProductsOnSaleAsync();
        //Task<Product> GetProductByIdAsync(int id);
        Task<List<Product>> GetRecentAsync(int take = 50);
    }
}
