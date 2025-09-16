//using ABCRetailApp.Data;
//using ABCRetailApp.Models;
//using Azure.Data.Tables;
//using Radzen.Blazor.Markdown;

//namespace ABCRetailApp.Services
//{
//    public class AzureTableService : IAzureTableService
//    {
//        private readonly TableClient _tableClient;

//        public AzureTableService(IConfiguration configuration)
//        {
//            var connectionString = configuration.GetConnectionString("AzureTableStorage");
//            var tableName = configuration["AzureTableStorage:TableName"] ?? "Products";

//            _tableClient = new TableClient(connectionString, tableName);
//        }

//        private TableClient Table =>
//          _tableClient ?? throw new InvalidOperationException("Repository not initialized. Call InitializeAsync().");
//        public async Task<IReadOnlyList<Product>> ListRecentAsync(int take = 50)
//        {
//            // Tables don't support rich ORDER BY queries. We'll fetch all and sort in memory.
//            var results = new List<Product>();

//            await foreach (var entity in Table.QueryAsync<ProductEntity>())
//            {
//                results.Add(entity.ToOrder());
//            }

//            return results
//                .OrderByDescending(o => o.CreatedUtc)
//                .Take(take)
//                .ToList();
//        }

//        public async Task<List<Product>> GetProductsAsync(string partitionKey = "Products")
//        {
//            try
//            {
//                var products = new List<Product>();
//                var query = _tableClient.QueryAsync<TableEntity>(filter: $"PartitionKey eq '{partitionKey}'");

//                await foreach (var entity in query)
//                {
//                    products.Add(ProductEntity.FromProduct(entity));
//                }

//                return products;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error retrieving products: {ex.Message}");
//                return new List<Product>();
//            }
//        }

//        public async Task<List<Product>> GetProductsOnSaleAsync()
//        {
//            try
//            {
//                var products = new List<Product>();
//                var filter = $"PartitionKey eq 'Products' and IsOnSale eq true";
//                var query = _tableClient.QueryAsync<TableEntity>(filter: filter);

//                await foreach (var entity in query)
//                {
//                    products.Add(Product.FromTableEntity(entity));
//                }

//                return products;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error retrieving products on sale: {ex.Message}");
//                return new List<Product>();
//            }
//        }

//        public async Task<Product> GetProductByIdAsync(string rowKey)
//        {
//            try
//            {
//                var response = await _tableClient.GetEntityAsync<TableEntity>("Products", rowKey);
//                return Product.FromTableEntity(response.Value);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error retrieving product {rowKey}: {ex.Message}");
//                return null;
//            }
//        }
//    }
//}
