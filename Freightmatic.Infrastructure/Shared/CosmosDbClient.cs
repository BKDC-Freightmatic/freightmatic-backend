using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace Freightmatic.Infrastructure.Shared
{
    public class CosmosDbClient
    {
        private readonly CosmosClient _client;
        private Container _container;
        private Database _database;

        public CosmosDbClient(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("CosmosDB")!;

            if (configuration["IsDevEnv"].ToLower() == "true")
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

        public async Task Init()
        {
            _database = await _client.CreateDatabaseIfNotExistsAsync(id: "feightmatic", throughput: 10000);
            _container = await _database.CreateContainerIfNotExistsAsync(id: "users", partitionKeyPath: "/partitionKey");
        }

        public async Task<ItemResponse<T>> CreateItemAsync<T>(T data)
        {
            ItemResponse<T> result = await _container.CreateItemAsync(data, new PartitionKey("feightmatic"));
            return result;
        }

        public async Task<ItemResponse<T>> UpdateItemAsync<T>(T data)
        {
            ItemResponse<T> result = await _container.UpsertItemAsync(data, new PartitionKey("feightmatic"));
            return result;
        }

        public async Task<T> GetItemByIdAsync<T>(string id)
        {
            ItemResponse<T> result = await _container.ReadItemAsync<T>(id, new PartitionKey("feightmatic"));
            return result;
        }

        public async Task<List<T>> GetItemsAsync<T>(string containerName, QueryDefinition queryDefinition)
        {
            List<T> items = [];

            Container container = await _database.CreateContainerIfNotExistsAsync(id: containerName, partitionKeyPath: "/partitionKey");
            using (FeedIterator<T> resultSetIterator = container.GetItemQueryIterator<T>(queryDefinition))
            {
                while (resultSetIterator.HasMoreResults)
                {
                    FeedResponse<T> response = await resultSetIterator.ReadNextAsync();
                    items.AddRange(response); // Add all matching items to the list
                }
            }

            return items; // Return the list of items found
        }

        public async Task<List<T>> GetItemsAsync<T>(QueryDefinition queryDefinition)
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