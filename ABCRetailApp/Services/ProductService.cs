using ABCRetailApp.Data;
using ABCRetailApp.Models;

namespace ABCRetailApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }
        //public Task<Product> GetProductByIdAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<List<Product>> GetProductsAsync()
        //{
        //    throw new NotImplementedException();
        //}
        public Task<List<Product>> GetRecentAsync(int take = 50) => _repo.ListRecentAsync(take);
    }
}
