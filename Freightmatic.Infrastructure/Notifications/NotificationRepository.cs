using Freightmatic.Domain.Notifications;
using Freightmatic.Infrastructure.Shared;
using Microsoft.Azure.Cosmos;

namespace Freightmatic.Infrastructure.Notifications
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly CosmosDbClient _cosmosDbClient;

        public NotificationRepository(CosmosDbClient cosmosDbClient)
        {
            _cosmosDbClient = cosmosDbClient;
        }

        public async Task<List<Notification?>> GetAllAsync()
        {
            var query = new QueryDefinition("SELECT * FROM notifications");
            List<Notification?> items = await _cosmosDbClient.GetItemsAsync<Notification?>("notifications", query);
            return items;
        }
    }
}
