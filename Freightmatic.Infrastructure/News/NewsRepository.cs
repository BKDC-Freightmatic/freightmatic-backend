using Freightmatic.Domain.News;
using Freightmatic.Infrastructure.Shared;
using Microsoft.Azure.Cosmos;

namespace Freightmatic.Infrastructure.News
{
    public class NewsRepository : INewsRepository
    {
        private readonly CosmosDbClient _cosmosDbClient;

        public NewsRepository(CosmosDbClient cosmosDbClient)
        {
            _cosmosDbClient = cosmosDbClient;
        }

        public async Task<List<Domain.News.News?>> GetAllAsync()
        {
            var query = new QueryDefinition("SELECT * FROM news");
            List<Domain.News.News?> items = await _cosmosDbClient.GetItemsAsync<Domain.News.News?>("news", query);
            return items;
        }
    }
}
