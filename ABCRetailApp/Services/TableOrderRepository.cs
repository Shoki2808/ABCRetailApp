using ABCRetailApp.Data;
using Azure;
using Azure.Data.Tables;
using ABCRetailApp.Models;

namespace ABCRetailApp.Services
{
    public class TableOrderRepository : IOrderRepository
    {
        private readonly TableServiceClient _serviceClient;
        private readonly string _tableName;
        private TableClient? _table;

        public TableOrderRepository(string storageConnectionString, string tableName)
        {
            _serviceClient = new TableServiceClient(storageConnectionString);
            _tableName = tableName;
        }

        public async Task InitializeAsync()
        {
            _table = _serviceClient.GetTableClient(_tableName);
            await _table.CreateIfNotExistsAsync();
        }

        private TableClient Table =>
            _table ?? throw new InvalidOperationException("Repository not initialized. Call InitializeAsync().");

        public async Task<Order?> GetAsync(string id, string customerId)
        {
            try
            {
                var response = await Table.GetEntityAsync<OrderEntity>(partitionKey: customerId, rowKey: id);
                return response.Value.ToOrder();
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return null;
            }
        }

        public async Task<IReadOnlyList<Order>> ListByCustomerAsync(string customerId)
        {
            var results = new List<Order>();

            await foreach (var entity in Table.QueryAsync<OrderEntity>(x => x.PartitionKey == customerId))
            {
                results.Add(entity.ToOrder());
            }

            return results.OrderByDescending(o => o.CreatedUtc).ToList();
        }

        public async Task<IReadOnlyList<Order>> ListRecentAsync(int take = 50)
        {
            // Tables don't support rich ORDER BY queries. We'll fetch all and sort in memory.
            var results = new List<Order>();

            await foreach (var entity in Table.QueryAsync<OrderEntity>())
            {
                results.Add(entity.ToOrder());
            }

            return results
                .OrderByDescending(o => o.CreatedUtc)
                .Take(take)
                .ToList();
        }

        public async Task CreateAsync(Order order)
        {
            var entity = OrderEntity.FromOrder(order);
            await Table.AddEntityAsync(entity);
        }

        public async Task UpdateAsync(Order order)
        {
            var entity = OrderEntity.FromOrder(order);
            await Table.UpsertEntityAsync(entity, TableUpdateMode.Replace);
        }

        public async Task DeleteAsync(string id, string customerId)
        {
            await Table.DeleteEntityAsync(partitionKey: customerId, rowKey: id);
        }
    }
}
