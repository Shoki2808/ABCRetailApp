//using Microsoft.Azure.Cosmos;
//using OrderingSystem.Models;

//namespace ABCRetailApp.Data;

//public class CosmosOrderRepository : IOrderRepository
//{
//    private readonly CosmosClient _client;
//    private readonly string _databaseName;
//    private readonly string _containerName;
//    private readonly string _partitionKeyPath;

//    private Container? _container;

//    public CosmosOrderRepository(CosmosClient client, string databaseName, string containerName, string partitionKeyPath)
//    {
//        _client = client;
//        _databaseName = databaseName;
//        _containerName = containerName;
//        _partitionKeyPath = partitionKeyPath;
//    }

//    public async Task InitializeAsync()
//    {
//        var db = await _client.CreateDatabaseIfNotExistsAsync(_databaseName);
//        var containerProperties = new ContainerProperties(_containerName, _partitionKeyPath);
//        var throughput = ThroughputProperties.CreateAutoscaleThroughput(4000);
//        var response = await db.Database.CreateContainerIfNotExistsAsync(containerProperties, throughput);

//        _container = response.Container;
//    }

//    private Container Container =>
//        _container ?? throw new InvalidOperationException("Repository not initialized. Call InitializeAsync().");

//    public async Task<Order?> GetAsync(string id, string partitionKey)
//    {
//        try
//        {
//            var resp = await Container.ReadItemAsync<Order>(id, new PartitionKey(partitionKey));
//            return resp.Resource;
//        }
//        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
//        {
//            return null;
//        }
//    }

//    public async Task<IReadOnlyList<Order>> ListByCustomerAsync(string customerId)
//    {
//        var query = new QueryDefinition("SELECT * FROM c WHERE c.customerId = @cid ORDER BY c.createdUtc DESC")
//            .WithParameter("@cid", customerId);
//        var iter = Container.GetItemQueryIterator<Order>(query);
//        var results = new List<Order>();
//        while (iter.HasMoreResults)
//        {
//            var page = await iter.ReadNextAsync();
//            results.AddRange(page);
//        }
//        return results;
//    }

//    public async Task<IReadOnlyList<Order>> ListRecentAsync(int take = 50)
//    {
//        var query = new QueryDefinition("SELECT TOP @take * FROM c ORDER BY c.createdUtc DESC")
//            .WithParameter("@take", take);
//        var iter = Container.GetItemQueryIterator<Order>(query);
//        var results = new List<Order>();
//        while (iter.HasMoreResults)
//        {
//            var page = await iter.ReadNextAsync();
//            results.AddRange(page);
//        }
//        return results;
//    }

//    public async Task CreateAsync(Order order)
//    {
//        await Container.CreateItemAsync(order, new PartitionKey(order.CustomerId));
//    }

//    public async Task UpdateAsync(Order order)
//    {
//        await Container.UpsertItemAsync(order, new PartitionKey(order.CustomerId));
//    }

//    public async Task DeleteAsync(string id, string partitionKey)
//    {
//        await Container.DeleteItemAsync<Order>(id, new PartitionKey(partitionKey));
//    }
//}
