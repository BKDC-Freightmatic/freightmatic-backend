using Freightmatic.Domain.Deliveries;
using Freightmatic.Infrastructure.Shared;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace Freightmatic.Infrastructure.Deliveries
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private CosmosClient _client;
        private Container _container;
        public DeliveryRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("CosmosDB")!;

            if (configuration["IsDevEnv"] == "true")
            {
                CosmosClientOptions options = new()
                {
                    HttpClientFactory = () => new HttpClient(new HttpClientHandler()
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    }),
                    ConnectionMode = ConnectionMode.Gateway,
                };
                _client = new(connectionString, options);
            }
            else
            {
                _client = new(connectionString);
            }

            Init().Wait();
        }

        private async Task Init()
        {
            Database database = await _client.CreateDatabaseIfNotExistsAsync(id: "feightmatic", throughput: 10000);
            _container = await database.CreateContainerIfNotExistsAsync(id: "deliveries", partitionKeyPath: "/partitionKey");
        }

        public async Task<bool> CreateDeliveryAsync(Delivery delivery)
        {
            ItemResponse<Delivery> response = await _container.CreateItemAsync(delivery, new PartitionKey("feightmatic"));
            return response.StatusCode == System.Net.HttpStatusCode.Created;
        }

        public async Task<List<Delivery?>> GetAllAsync()
        {
            var query = new QueryDefinition("SELECT * FROM deliveries");
            List<Delivery?> items = await GetItemsAsync<Delivery?>(query);
            return items;
        }

        public async Task<Delivery> GetDeliveryAsync(string deliveryId)
        {
            Delivery result = await _container.ReadItemAsync<Delivery>(deliveryId, new PartitionKey("feightmatic"));
            return result;
        }

        public async Task<bool> UpdateDeliveryAsync(Delivery delivery)
        {
            ItemResponse<Delivery> response = await _container.UpsertItemAsync(delivery, new PartitionKey("feightmatic"));
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        private async Task<List<T>> GetItemsAsync<T>(QueryDefinition queryDefinition)
        {
            List<T> items = [];

            using (FeedIterator<T> resultSetIterator = _container.GetItemQueryIterator<T>(queryDefinition))
            {
                while (resultSetIterator.HasMoreResults)
                {
                    FeedResponse<T> response = await resultSetIterator.ReadNextAsync();
                    items.AddRange(response); // Add all matching items to the list
                }
            }

            return items; // Return the list of items found
        }
    }
}
