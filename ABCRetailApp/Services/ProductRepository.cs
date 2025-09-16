using ABCRetailApp.Data;
using ABCRetailApp.Models;
using Azure.Data.Tables;

namespace ABCRetailApp.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly TableServiceClient _serviceClient;
        private readonly string _tableName;
        private TableClient? _table;
        public ProductRepository(string storageConnectionString, string tableName)
        {
            _serviceClient = new TableServiceClient(storageConnectionString);
            _tableName = tableName;
        }

        public async Task InitializeAsync()
        {
            _table = _serviceClient.GetTableClient(_tableName);
            //await _table.CreateIfNotExistsAsync();
        }
        private TableClient Table =>
           _table ?? throw new InvalidOperationException("Repository not initialized. Call InitializeAsync().");

        public async Task<List<Product>> ListRecentAsync(int take = 50)
        {
            // Tables don't support rich ORDER BY queries. We'll fetch all and sort in memory.
            var results = new List<Product>();

            await foreach (var entity in Table.QueryAsync<ProductEntity>())
            {
                results.Add(entity.ToProduct());
            }

            return results
                .Take(take)
                .ToList();
        }
    }
}
